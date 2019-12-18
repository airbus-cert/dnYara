using System;
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

        public CompiledRules(IntPtr rulesPtr)
        {
            BasePtr = rulesPtr;
        }

        public CompiledRules(string filename)
        {
            IntPtr ptr = IntPtr.Zero;
            ErrorUtility.ThrowOnError(Methods.yr_rules_load(filename, ref ptr));
            BasePtr = ptr;
        }

        ~CompiledRules()
        {
            Dispose();
        }

        public YR_RULES GetStruct()
        {
            YR_RULES yrRule = Marshal.PtrToStructure<YR_RULES>(BasePtr);
            return yrRule;
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