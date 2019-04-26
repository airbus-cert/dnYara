using System;
using Mono.Unix.Native;
using dnYara.Exceptions;

namespace dnYara
{
    /// <summary>
    /// RAII wrapper for POSIX file handling.
    /// </summary>
    public class PosixFileHandler 
        : IDisposable
    {
        public IntPtr FileHandle { get; }

        public PosixFileHandler(string path, string mode)
        {
            FileHandle = Stdlib.fopen(path, mode);

            if (FileHandle == IntPtr.Zero)
            {
                Errno err = Stdlib.GetLastError();
                throw new FileIOException(err, path);
            }
        }

        public void Dispose()
        {
            if (!FileHandle.Equals(default(IntPtr)))
                Stdlib.fclose(FileHandle);
        }
        

        ~PosixFileHandler()
        {
            Dispose();
        }
    }
}
