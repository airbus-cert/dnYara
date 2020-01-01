using System;
using System.Runtime.InteropServices;


namespace dnYara.Interop
{

    /// <summary>
    /// Data structure representing a metadata value. Based on `type`, zero or one of `integerValue` or `stringValue` will be filled,
    /// and if `type` is `META_TYPE_BOOLEAN` then `integerValue` should be parsed as a boolean
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct YR_META
    {

        /// <summary>
        /// One of the following metadata types:
        /// <code>META_TYPE_NULL META_TYPE_INTEGER META_TYPE_STRING META_TYPE_BOOLEAN</code>
        /// </summary>
        public int type;
        
        public long integerValue;

        /// <summary>
        /// Meta identifier.
        /// </summary>
        public IntPtr identifier;
        
        public IntPtr stringValue;
    }

}
