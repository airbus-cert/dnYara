using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using dnYara.Interop;

namespace dnYara
{
    /// <summary>
    /// Data structure representing a single rule.
    /// </summary>
    public sealed class Rule
    {
        /// <summary>
        /// Rule identifier.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Pointer to a sequence of null terminated strings with tag names. An additional null character
        /// marks the end of the sequence. Example: tag1\0tag2\0tag3\0\0.
        /// To iterate over the tags you can use yr_rule_tags_foreach().
        /// </summary>
        public List<string> Tags { get; set; }
        
        public Rule()
        {
            Identifier = string.Empty;
            Tags = new List<string>();
        }

        public Rule(YR_RULE rule)
        {
            IntPtr ptr = rule.identifier;
            Identifier = Marshal.PtrToStringAnsi(ptr);

            Tags = new List<string>();
            
            ObjRefHelper.ForEachStringInObjRef(rule.tags, Tags.Add);
        }
    }
}