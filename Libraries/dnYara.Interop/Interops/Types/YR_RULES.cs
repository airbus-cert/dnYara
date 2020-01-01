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
        /// unsigned char[4]
        [FieldOffset(0)]
        public fixed byte tidx_mask[4];

        /// uint8_t*
        [FieldOffset(8)]
        public IntPtr code_start;

        /// YR_MUTEX->(HANDLE->void* or pthread_mutex_t)
        [FieldOffset(16)]
        public OpaquePthreadMutexT mutex;

        /// YR_ARENA*
        [FieldOffset(80)]
        public IntPtr arena;

        /// YR_RULE*
        [FieldOffset(88)]
        public IntPtr rules_list_head;

        /// YR_EXTERNAL_VARIABLE*
        [FieldOffset(96)]
        public IntPtr externals_list_head;

        /// YR_AC_TRANSITION_TABLE->YR_AC_TRANSITION*
        [FieldOffset(104)]
        public IntPtr ac_transition_table;

        /// YR_AC_MATCH_TABLE->YR_AC_MATCH_TABLE_ENTRY*
        [FieldOffset(112)]
        public IntPtr ac_match_table;

        // Size of ac_match_table and ac_transition_table in number of items (both
        // tables have the same numbe of items).
        [FieldOffset(120)]
        public uint ac_tables_size;

        // Used only when PROFILING_ENABLED is defined.
        [FieldOffset(128)]
        public ulong time_cost;
    }

}
