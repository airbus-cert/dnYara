using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{
    public static class ObjRefHelper
    {
        private static int POINTER_SIZE = Marshal.SizeOf(IntPtr.Zero);

        /// iterates over a linked-list of YR_STRINGs, starting from a given location.
        /// performs the equivalent of `yr_rule_strings_foreach`.
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

        public static bool StringIsLastInRule(YR_STRING str) {
            return (str.flags & Constants.STRING_FLAGS_LAST_IN_RULE) != 0;
        }
        public static bool CheckYRString(IntPtr yrStringPtr, out YR_STRING yrString)
        {
            yrString = default;

            if (yrStringPtr == IntPtr.Zero)
                return false;

            yrString = (YR_STRING)Marshal.PtrToStructure(yrStringPtr, typeof(YR_STRING));

            if (yrString.identifier == IntPtr.Zero || StringIsLastInRule(yrString))
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
            EachStructOfTInObjRef<YR_META>(yrMetasPtr, meta => meta.type != (int) META_TYPE.META_TYPE_NULL);

        public static string GetYRString(IntPtr objRef)
        {
            string outStr;
            CheckTag(objRef, out outStr);
            return outStr;
        }

        /// implements the header-only function`yr_string_matches_foreach` for iterating through
        /// matches in a scan.
        public static void ForEachStringMatches(YR_SCAN_CONTEXT scan_context, YR_STRING str, Action<YR_MATCH> p)
        {
            var str_matches_offset = (int)str.idx * Marshal.SizeOf(typeof(YR_MATCHES));
            var initMatchPtr = (YR_MATCHES)Marshal.PtrToStructure(scan_context.matches + str_matches_offset, typeof(YR_MATCHES));
            YR_MATCH yrMatch;

            for (var matchPtr = initMatchPtr.head;
                !matchPtr.Equals(IntPtr.Zero);
                matchPtr = yrMatch.next)
            {
                yrMatch = GetMatchFromObjRef(matchPtr);
                if (yrMatch.is_private)
                {
                    continue;
                }

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

        // replicates the RULE_IS_NULL check from the types.h module of yara.
        // used in rule iteration.
        public static bool RuleIsNull(YR_RULE rule) {
            return (rule.flags & Constants.RULE_FLAGS_NULL) != 0;
        }
    }
}
