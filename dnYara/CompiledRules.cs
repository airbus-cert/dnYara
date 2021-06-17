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

        public uint RuleCount { get; private set; }
        public uint StringsCount { get; private set; }
        public uint NamespacesCount { get; private set; }

        public CompiledRules(IntPtr rulesPtr)
        {
            BasePtr = rulesPtr;
            ExtractData();
        }

        private void ExtractData()
        {
            var ruleStruct = Marshal.PtrToStructure<YR_RULES>(BasePtr);
            Rules = ObjRefHelper
                .GetRules(ruleStruct.rules_list_head)
                .Select(rule => new Rule(rule))
                .ToList();
            RuleCount = ruleStruct.num_rules;
            StringsCount = ruleStruct.num_strings;
            NamespacesCount = ruleStruct.num_namespaces;
        }
        public CompiledRules(string filename)
        {
            IntPtr ptr = IntPtr.Zero;
            ErrorUtility.ThrowOnError(Methods.yr_rules_load(filename, ref ptr));
            BasePtr = ptr;
            ExtractData();
        }

        ~CompiledRules()
        {
            if (BasePtr != default)
                Release();
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
            BasePtr = default;
            return temp;
        }
    }
}
