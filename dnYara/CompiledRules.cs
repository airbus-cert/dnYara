using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using dnYara.Interop;

namespace dnYara
{
    /// <summary>
    /// Yara compiled rules.
    /// </summary>
    public sealed class CompiledRules
        : IDisposable
    {
        internal IntPtr BasePtr { get; set; }

        public List<Rule> Rules { get; private set; }

        public CompiledRules(IntPtr rulesPtr)
        {
            BasePtr = rulesPtr;
            var ruleStruct = Marshal.PtrToStructure<YR_RULES>(BasePtr);
            Rules = ObjRefHelper.GetRules(ruleStruct.rules_list_head).Select(rule => new Rule(rule)).ToList();
        }

        public CompiledRules(string filename)
        {
            IntPtr ptr = IntPtr.Zero;
            ErrorUtility.ThrowOnError(Methods.yr_rules_load(filename, ref ptr));
            BasePtr = ptr;
            var ruleStruct = Marshal.PtrToStructure<YR_RULES>(BasePtr);
            Rules = ObjRefHelper.GetRules(ruleStruct.rules_list_head).Select(rule => new Rule(rule)).ToList();
        }

        ~CompiledRules()
        {
            Dispose();
        }

        public bool Save(string filename)
        {
            ErrorUtility.ThrowOnError(Methods.yr_rules_save(BasePtr, filename));
            return true;
        }

        public void Dispose()
        {
            if (!BasePtr.Equals(IntPtr.Zero))
            {
                IntPtr ptr = BasePtr;
                BasePtr = IntPtr.Zero;
                Methods.yr_rules_destroy(ptr);
            }
        }

        public IntPtr Release()
        {
            var temp = BasePtr;
            BasePtr = default(IntPtr);
            return temp;
        }
    }
}