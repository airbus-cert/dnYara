using System;
using System.Runtime.InteropServices;


namespace dnYara.Interop
{
    /// <summary>
    /// Data structure representing a metadata value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct YR_MATCH
    {

        /// <summary>
        /// Base offset/address for the match. While scanning a file this field is usually zero, while scanning a
        /// process memory space this field is the virtual address of the memory block where the match was found.
        /// </summary>
        public long @base;

        /// <summary>
        /// Offset of the match relative to base.
        /// </summary>
        public long offset;

        /// <summary>
        /// Length of the matching string
        /// </summary>
        public int match_length;

        /// <summary>
        /// Length of data buffer. data_length is the minimum of match_length and MAX_MATCH_DATA.
        /// </summary>
        public int data_length;

        /// <summary>
        /// Pointer to a buffer containing a portion of the matching string.
        /// </summary>
        public IntPtr dataPtr;
        
        public int chain_length;

        /// YR_MATCH*
        public IntPtr prev;

        /// YR_MATCH*
        public IntPtr next;
    }

}
