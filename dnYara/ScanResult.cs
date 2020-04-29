using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using dnYara.Interop;

namespace dnYara
{
    public class ScanResult
    {
        public Rule MatchingRule;
        public Dictionary<string, List<Match>> Matches;

        public ScanResult()
        {
            MatchingRule = null;
            Matches = new Dictionary<string, List<Match>>();
        }

        public ScanResult(YR_SCAN_CONTEXT scanContext, YR_RULE matchingRule)
        {
            MatchingRule = new Rule(matchingRule);
            Matches = new Dictionary<string, List<Match>>();

            ObjRefHelper.ForEachYaraStringInObjRef(matchingRule.strings, str =>
            {
                var identifier = str.identifier;

                if (identifier == IntPtr.Zero)
                    return;

                ObjRefHelper.ForEachStringMatches(scanContext, str, match =>
                {
                    string matchText = ObjRefHelper.GetYRString(identifier);

                    if (!Matches.ContainsKey(matchText))
                        Matches.Add(matchText, new List<Match>());

                    Matches[matchText].Add(new Match(match));
                });
            });

        }
    }
}
