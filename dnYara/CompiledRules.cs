using System;
using System.Collections.Generic;
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
        private YR_RULES _struct = default(YR_RULES);
        
        public List<Rule> Rules { get; private set; }

        public CompiledRules(IntPtr rulesPtr)
        {
            BasePtr = rulesPtr;
            _struct = Marshal.PtrToStructure<YR_RULES>(BasePtr);
            Rules = new List<Rule>();
            Console.WriteLine($"Marshalled YR_RULES struct, cost {_struct.time_cost}");
            ObjRefHelper.ForEachStructOfTInObjRef<YR_RULE>(
                _struct.rules_list_head, 
                rule => {
                    Console.WriteLine($"Checking rule {rule.identifier} with flags {rule.g_flags}");
                    var result = ObjRefHelper.RuleIsNull(rule);
                    Console.WriteLine($"Rule {rule.identifier} is null: {result}");
                    return !result;
                },
                rule => {
                    Console.WriteLine($"Creating new Rule for {rule.identifier}");
                    var created = new Rule(rule);
                    Console.WriteLine($"Created new rule {created.Identifier}");
                    Rules.Add(created);
                });
        }

        public CompiledRules(string filename)
        {
            IntPtr ptr = IntPtr.Zero;
            ErrorUtility.ThrowOnError(Methods.yr_rules_load(filename, ref ptr));
            BasePtr = ptr;
            _struct = Marshal.PtrToStructure<YR_RULES>(BasePtr);
            Rules = new List<Rule>();
            ObjRefHelper.ForEachStructOfTInObjRef<YR_RULE>(
                _struct.rules_list_head, 
                rule => {
                    Console.WriteLine($"Checking rule {rule.identifier} with flags {rule.g_flags}");
                    var result = ObjRefHelper.RuleIsNull(rule);
                    Console.WriteLine($"Rule {rule.identifier} is null: {result}");
                    return !result;
                },
                rule => {
                    Console.WriteLine($"Creating new Rule for {rule.identifier}");
                    var created = new Rule(rule);
                    Console.WriteLine($"Created new rule {created.Identifier}");
                    Rules.Add(created);
                });
        }

        ~CompiledRules()
        {
            Dispose();
        }

        public YR_RULES GetStruct()
        {
            return _struct;
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