using System;
using dnYara.Interop;

namespace dnYara.Exceptions
{
    public sealed class YaraException
        : Exception
    {
        public YARA_ERROR YRError { get; set; }
        public YaraException(YARA_ERROR error)
                : base(string.Format("Yara error code {0}", Enum.GetName(typeof(YARA_ERROR), error)))
        {
            YRError = error;
        }
    }
}
