using System;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{
    [StructLayout(LayoutKind.Explicit, Size = 448)]
    public unsafe struct YR_RULE
    {
        // Global flags
        [FieldOffset(0)]
        public int g_flags;

        // Thread-specific flags
        [FieldOffset(4)]
        public fixed int t_flags[32];

        [FieldOffset(136)]
        public IntPtr identifier;

        [FieldOffset(144)]
        public IntPtr tags;

        [FieldOffset(152)]
        public IntPtr metas;

        [FieldOffset(160)]
        public IntPtr strings;

        [FieldOffset(168)]
        public IntPtr ns;

        // Number of atoms generated for this rule.
        [FieldOffset(176)]
        public int num_atoms;

        // Used only when PROFILING_ENABLED is defined. This is the sum of all values
        // in time_cost_per_thread. This is updated once on each call to
        // yr_scanner_scan_xxx.
        [FieldOffset(184)]
        public long time_cost;

        // Used only when PROFILING_ENABLED is defined. This array holds the time
        // cost for each thread using this structure concurrenlty. This is necessary
        // because a global variable causes too much contention while trying to
        // increment in a synchronized way from multiple threads.
        [FieldOffset(192)]
        public fixed long time_cost_per_thread[32];
    }

}
