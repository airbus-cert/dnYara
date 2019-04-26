using System;
using dnYara.Interop;

namespace dnYara
{
    /// <summary>
    /// RAII wrapper for Yara context, must be used with 'using' keyword. 
    /// </summary>
    public sealed class YaraContext 
        : IDisposable
    {
        public YaraContext()
        {
            ErrorUtility.ThrowOnError(Methods.yr_initialize());
        }

        ~YaraContext()
        {
            Dispose();
        }

        public void Dispose()
        {
            Methods.yr_finalize();
        }
    }
    
}
