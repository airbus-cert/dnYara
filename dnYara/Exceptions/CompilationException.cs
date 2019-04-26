using System;
using System.Collections.Generic;

namespace dnYara.Exceptions
{
    public sealed class CompilationException
        : Exception
    {
        public List<string> Errors;

        public CompilationException(List<string> errors)
            : base(string.Format(
                        "Error compiling rules.\n{0}", string.Join("\n", errors)))
        {
            Errors = new List<string>(errors);
        }
    }
}
