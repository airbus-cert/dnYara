using System.Linq;
using System.Collections.Generic;

namespace dnYara {
    public static class DictionaryExtensions {
        public static IDictionary<Key, Value> ToDictionary<Key, Value>(this IEnumerable<(Key,Value)> values) {
            var dict = new Dictionary<Key, Value>();
            foreach (var (key, value) in values) {
                dict[key] = value;
            }
            return dict;
        } 
    }
}