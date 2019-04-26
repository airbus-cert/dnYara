using System;
using System.Runtime.InteropServices;


namespace dnYara.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct YR_RULES
    {

        /// unsigned char[4]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.tidx_mask_size, ArraySubType = UnmanagedType.I1)]
        public char[] tidx_mask;

        /// uint8_t*
        public IntPtr code_start;

        /// YR_MUTEX->HANDLE->void*
        public IntPtr mutex;

        /// YR_ARENA*
        public IntPtr arena;

        /// YR_RULE*
        public IntPtr rules_list_head;

        /// YR_EXTERNAL_VARIABLE*
        public IntPtr externals_list_head;

        /// YR_AC_TRANSITION_TABLE->YR_AC_TRANSITION*
        public IntPtr ac_transition_table;

        /// YR_AC_MATCH_TABLE->YR_AC_MATCH_TABLE_ENTRY*
        public IntPtr ac_match_table;

        // Size of ac_match_table and ac_transition_table in number of items (both
        // tables have the same numbe of items).
        public UInt32 ac_tables_size;

        // Used only when PROFILING_ENABLED is defined.
        public UInt64 time_cost;
    }

}
