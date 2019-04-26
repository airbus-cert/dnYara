using System;
using System.Runtime.InteropServices;


namespace dnYara.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YR_STRING
    {

        /// int
        public int g_flags;

        /// int
        public int length;

        /// Anonymous_58a296e1_fd92_47bc_a3b7_3ad5f0f9a75a
        public IntPtr identifier;

        /// Anonymous_c6f950a6_e9f8_4b17_b46c_1c7c15c53e6f
        public IntPtr string_;

        /// Anonymous_f516663e_8137_4d9c_87d8_b8c32d6da80a
        public IntPtr chained_to;

        /// Anonymous_d9665912_7174_4b74_ad97_bcc2c059c8dc
        public IntPtr rule;

        /// int
        public int chain_gap_min;

        /// int
        public int chain_gap_max;

        /// int
        public long fixed_offset;

        /// YR_MATCHES[32]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32/*, ArraySubType = UnmanagedType.Struct*/)]
        public YR_MATCHES[] matches;

        /// YR_MATCHES[32]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32/*, ArraySubType = UnmanagedType.Struct*/)]
        public YR_MATCHES[] unconfirmed_matches;
    }

}
