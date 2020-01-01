using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{
    public static class ObjRefHelper
    {
        private static int POINTER_SIZE = Marshal.SizeOf(IntPtr.Zero);

        public static void ForEachYaraStringInObjRef(IntPtr ref_obj, Action<YR_STRING> action)
        {
            YR_STRING yrString;
            for (
                IntPtr yrStringPtr = ref_obj;
                CheckYRString(yrStringPtr, out yrString);
                yrStringPtr += POINTER_SIZE)
            {
                action(yrString);
            }
        }

        public static bool CheckYRString(IntPtr yrStringPtr, out YR_STRING yrString)
        {
            yrString = default;

            if (yrStringPtr == IntPtr.Zero)
                return false;

            yrString = (YR_STRING)Marshal.PtrToStructure(yrStringPtr, typeof(YR_STRING));

            if (yrString.identifier == IntPtr.Zero || yrString.g_flags == 0)
                return false;

            return true;
        }

        public static void ForEachStringInObjRef(IntPtr ref_obj, Action<string> action)
        {
            string tagName;
            for (
                IntPtr tagNamePtr = ref_obj;
                CheckTag(tagNamePtr, out tagName);
                tagNamePtr += tagName.Length + 1)
            {
                action(tagName);
            }
        }

        /// walks a variable-sized array of pointers of type T, marshalling and running a custom validation function on each iteration of the pointer
        /// This is an abstraction around specialized loops like `ForEachYaraMetaInObjRef`
        public static IEnumerable<T> EachStructOfTInObjRef<T>(IntPtr ref_obj, Func<T, bool> validityChecker) where T: struct {
            T structPtr;
            for (
                IntPtr structArrayPtr = ref_obj;
                MarshalAndValidate(structArrayPtr, validityChecker, out structPtr);
                structArrayPtr += Marshal.SizeOf(typeof(T)))
            {
                yield return structPtr;
            }
        }

        public static IEnumerable<YR_RULE> GetRules(IntPtr rulesPtr) => 
            EachStructOfTInObjRef<YR_RULE>(rulesPtr, rule => {
                var result = ObjRefHelper.RuleIsNull(rule);
                return !result && rule.identifier != IntPtr.Zero;
            });
        
        public static IEnumerable<YR_META> GetMetas(IntPtr yrMetasPtr) => 
            EachStructOfTInObjRef<YR_META>(yrMetasPtr, meta => meta.type != 0);

        public static string GetYRString(IntPtr objRef)
        {
            string outStr;
            CheckTag(objRef, out outStr);
            return outStr;
        }

        public static void ForEachStringMatches(YR_STRING str, Action<YR_MATCH> p)
        {
            int idx = Methods.yr_get_tidx();
            var initMatchPtr = str.matches[idx].head;
            YR_MATCH yrMatch;

            for (var matchPtr = initMatchPtr;
                !matchPtr.Equals(IntPtr.Zero);
                matchPtr = yrMatch.next)
            {
                yrMatch = GetMatchFromObjRef(matchPtr);

                p(yrMatch);

                if (yrMatch.next == IntPtr.Zero)
                    return;
            }
        }

        public static YR_MATCH GetMatchFromObjRef(IntPtr objRef)
        {
            try
            {
                YR_MATCH yrMatch = (YR_MATCH)Marshal.PtrToStructure(objRef, typeof(YR_MATCH));
                return yrMatch;
            }
            catch
            {
                Debug.WriteLine($"Error for Match : {objRef}");
                return default;
            }
        }

        public static bool MarshalAndValidate<T>(IntPtr struct_ptr, Func<T, bool> validityChecker, out T destination_ptr) where T : struct {
            destination_ptr = default(T);
            if (struct_ptr == IntPtr.Zero) {
                return false;
            }

            destination_ptr = (T)Marshal.PtrToStructure(struct_ptr, typeof(T));
            return validityChecker(destination_ptr);
        }

        public static bool CheckTag(IntPtr tag_ptr, out string tagName)
        {

            tagName = null;
            if (tag_ptr == IntPtr.Zero)
                return false;

            tagName = Marshal.PtrToStringAnsi(tag_ptr);
            if (string.IsNullOrEmpty(tagName))
                return false;

            return true;

        }

        public static readonly int RULE_GFLAGS_NULL = 0x1000;

        // replicates the RULE_IS_NULL check from the types.h module of yara.
        // used in rule iteration.
        public static bool RuleIsNull(YR_RULE rule) {
            return (rule.g_flags & RULE_GFLAGS_NULL) != 0;
        }
    }
}
