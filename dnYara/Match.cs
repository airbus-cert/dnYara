using System.Runtime.InteropServices;
using System.Text;
using dnYara.Interop;

namespace dnYara
{
    /// <summary>
    /// Data structure representing a string match.
    /// </summary>
    public sealed class Match
    {
        /// <summary>
        /// Base offset/address for the match. While scanning a file this field is usually zero, while
        /// scanning a process memory space this field is the virtual address of the memory block where the match was found.
        /// </summary>
        public long Base { get; set; }

        /// <summary>
        /// Offset of the match relative to base.
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// Buffer containing a portion of the matching string.
        /// </summary>
        public byte[] Data { get; set; }
        

        public Match(YR_MATCH match)
        {
            Base = match.@base;
            Offset = match.offset;

            Data = new byte[match.data_length];
            Marshal.Copy(match.dataPtr, Data, 0, Data.Length);
        }
        
        public override string ToString()
        {
            if (Data.Length == 0)
                return string.Empty;

            if (Data.Length > 1)
            {
                if (Data[0] == 0)
                    return Encoding.BigEndianUnicode.GetString(Data);

                else if (Data[1] == 0)
                    return Encoding.Unicode.GetString(Data);
            }

            return Encoding.ASCII.GetString(Data);
        }
    }
}

