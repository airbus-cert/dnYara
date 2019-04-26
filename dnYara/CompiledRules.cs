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

        ~CompiledRules()
        {
            Dispose();
        }

        public YR_RULES GetStruct()
        {
            YR_RULES yrRule = Marshal.PtrToStructure<YR_RULES>(BasePtr);
            return yrRule;
        }

        public void Dispose()
        {
            if (BasePtr.Equals(IntPtr.Zero))
                Methods.yr_rules_destroy(BasePtr);
        }
        
        public IntPtr Release()
        {
            var temp = BasePtr;
            BasePtr = default(IntPtr);

            return temp;
        }
    }
}