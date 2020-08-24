using System;
using System.Collections.Generic;
using System.Text;

namespace dnYara
{
    /// <summary>
    /// Data structure containing the different types of external variables passed to a custom scanner
    /// </summary>
    public class ExternalVariables
    {
        public Dictionary<string, string> StringVariables = new Dictionary<string, string>();

        public Dictionary<string, long> IntVariables = new Dictionary<string, long>();

        public Dictionary<string, double> FloatVariables = new Dictionary<string, double>();

        public Dictionary<string, bool> BoolVariables = new Dictionary<string, bool>();

        public void ClearAll()
        {
            StringVariables.Clear();
            IntVariables.Clear();
            FloatVariables.Clear();
            BoolVariables.Clear();
        }

        public int CountAll()
        {
            return StringVariables.Count + IntVariables.Count + FloatVariables.Count + BoolVariables.Count;
        }

    }
}
