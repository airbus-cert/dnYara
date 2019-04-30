using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{
    public static class ObjRefHelper
    {
        public static void ForEachYaraStringInObjRef(IntPtr ref_obj, Action<YR_STRING> action)
        {
            YR_STRING yrString;
            for (
                IntPtr yrStringPtr = ref_obj;
                CheckYRString(yrStringPtr, out yrString);
                yrStringPtr += Marshal.SizeOf(yrStringPtr))
            {
                action(yrString);
            }
        }

        public static void ForEachYaraMetaInObjRef(IntPtr ref_obj, Action<YR_META> action)
        {
            YR_META yrMeta;
            for (
                IntPtr yrStringPtr = ref_obj;
                CheckYRMeta(yrStringPtr, out yrMeta);
                yrStringPtr += 4)
            {
                action(yrMeta);
            }
        }

        public static bool CheckYRMeta(IntPtr yrMetasPtr, out YR_META yrMetas)
        {
            yrMetas = default;

            if (yrMetasPtr == IntPtr.Zero)
                return false;

            yrMetas = (YR_META)Marshal.PtrToStructure(yrMetasPtr, typeof(YR_META));

            if (yrMetas.type == 0)
                return false;

            return true;
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
    }
}
