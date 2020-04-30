using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using dnYara.Interop;
using System.Linq;

namespace dnYara
{

    public class ScanResult
    {
        public Rule MatchingRule;
        public Dictionary<string, List<Match>> Matches;
        public ProfilingInfo ProfilingInfo;

        public ScanResult()
        {
            MatchingRule = null;
            Matches = new Dictionary<string, List<Match>>();
            ProfilingInfo = null;
        }

        public ScanResult(YR_SCAN_CONTEXT scanContext, YR_RULE matchingRule)
        {
            MatchingRule = new Rule(matchingRule);
            Matches = new Dictionary<string, List<Match>>();

            var matchingStrings = ObjRefHelper.GetYaraStrings(matchingRule.strings);
            foreach (var str in matchingStrings)
            {
                var identifier = str.identifier;

                if (identifier == IntPtr.Zero)
                    return;

                var matches = ObjRefHelper.GetStringMatches(scanContext, str);

                foreach (var match in matches)
                {
                    string matchText = ObjRefHelper.ReadYaraString(str);

                    if (!Matches.ContainsKey(matchText))
                        Matches.Add(matchText, new List<Match>());

                    Matches[matchText].Add(new Match(match));
                    if (ProfilingInfo == null)
                    {
                        var profInfo = ObjRefHelper.TryGetProfilingInfoForRule(scanContext, (int)str.rule_idx);
                        if (profInfo.HasValue)
                        {
                            ProfilingInfo = new ProfilingInfo(profInfo.Value);
                        }
                    }
                }
            }
        }
    }
}
