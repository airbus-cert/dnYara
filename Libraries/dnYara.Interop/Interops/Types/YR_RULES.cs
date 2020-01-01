using System;
using System.Runtime.InteropServices;


namespace dnYara.Interop
{
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public unsafe struct OpaquePthreadMutexT
    {
        [FieldOffset(0)]
        public long __sig;

        [FieldOffset(8)]
        public fixed sbyte __opaque[56];
    }

    [StructLayout(LayoutKind.Explicit, Size = 136)]
    public unsafe struct YR_RULES
    {   
        [FieldOffset(0)]
        public fixed byte tidx_mask[4];

        [FieldOffset(8)]
        public global::System.IntPtr code_start;

        [FieldOffset(16)]
        public OpaquePthreadMutexT mutex;

        [FieldOffset(80)]
        public global::System.IntPtr arena;

        [FieldOffset(88)]
        public global::System.IntPtr rules_list_head;

        [FieldOffset(96)]
        public global::System.IntPtr externals_list_head;

        [FieldOffset(104)]
        public global::System.IntPtr ac_transition_table;

        [FieldOffset(112)]
        public global::System.IntPtr ac_match_table;

        [FieldOffset(120)]
        public uint ac_tables_size;

        [FieldOffset(128)]
        public ulong time_cost;
    }

}
