using System;
using dnYara.Interop;

namespace dnYara
{
    /// <summary>
    /// RAII wrapper for Yara context, must be used with 'using' keyword. 
    /// </summary>
    public sealed class YaraContext
    {
        private static readonly Lazy<YaraContext> lazy =
            new Lazy<YaraContext>(() => new YaraContext());

        public static YaraContext Instance { get { return lazy.Value; } }

        private bool isCleanedUp = false;

        private YaraContext()
        {
            ErrorUtility.ThrowOnError(Methods.yr_initialize());
        }

        ~YaraContext()
        {
            Cleanup();
        }

        public void Cleanup()
        {
            if (!isCleanedUp)
            {
                Methods.yr_finalize();
                isCleanedUp = true;
            }
        }

        public void EnsureInitialized()
        {
            if (isCleanedUp)
            {
                throw new InvalidOperationException("YaraContext has been cleaned up.");
            }
        }

        public void Reinitialize()
        {
            if (isCleanedUp)
            {
                ErrorUtility.ThrowOnError(Methods.yr_initialize());
                isCleanedUp = false;
            }
        }
    }

}
