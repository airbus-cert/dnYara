using System;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YR_RULE
    {
        public Int32 g_flags;       // Global flags
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.YR_MAX_THREADS, ArraySubType = UnmanagedType.I4)]
        public Int32[] t_flags;     // Thread-specific flags
        
        public IntPtr identifier;
        public IntPtr tags;
        public IntPtr metas;
        public IntPtr strings;
        public IntPtr ns;

        // Number of atoms generated for this rule.
        public Int32 num_atoms;

        // Used only when PROFILING_ENABLED is defined. This is the sum of all values
        // in time_cost_per_thread. This is updated once on each call to
        // yr_scanner_scan_xxx.
        public Int64 time_cost;

        // Used only when PROFILING_ENABLED is defined. This array holds the time
        // cost for each thread using this structure concurrenlty. This is necessary
        // because a global variable causes too much contention while trying to
        // increment in a synchronized way from multiple threads.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.YR_MAX_THREADS, ArraySubType = UnmanagedType.I8)]
        public Int64[] time_cost_per_thread;
    }

}
