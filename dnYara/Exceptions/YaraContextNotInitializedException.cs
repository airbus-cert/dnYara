using System;
using System.Collections.Generic;
using System.Text;

namespace dnYara.Exceptions
{
    public class YaraContextNotInitializedException : InvalidOperationException
    {
        public YaraContextNotInitializedException()
            : base("YaraContext has been cleaned up.")
        {
        }
    }
}
