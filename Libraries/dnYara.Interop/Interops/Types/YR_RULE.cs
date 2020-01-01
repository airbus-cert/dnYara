using System;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{
    [StructLayout(LayoutKind.Explicit, Size = 448)]
    public unsafe struct YR_RULE
    {
        [FieldOffset(0)]
        public int g_flags;

        [FieldOffset(4)]
        public fixed int t_flags[32];

        [FieldOffset(136)]
        public global::System.IntPtr identifier;

        [FieldOffset(144)]
        public global::System.IntPtr tags;

        [FieldOffset(152)]
        public global::System.IntPtr metas;

        [FieldOffset(160)]
        public global::System.IntPtr strings;

        [FieldOffset(168)]
        public global::System.IntPtr ns;

        [FieldOffset(176)]
        public int num_atoms;

        [FieldOffset(184)]
        public long time_cost;

        [FieldOffset(192)]
        public fixed long time_cost_per_thread[32];
    }

}
