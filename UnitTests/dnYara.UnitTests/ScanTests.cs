using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace dnYara.UnitTests
{
    public class ScanTests
    {
        [Fact]
        public void CheckStringMatchTest()
        {
            // Initialize yara context
            using (YaraContext ctx = new YaraContext())
            {
                // Compile yara rules
                CompiledRules rules = null;

                using (var compiler = new Compiler())
                {
                    compiler.AddRuleString("rule foo: bar {strings: $a = \"lmn\" condition: $a}");

                    rules = compiler.Compile();
                }

                if (rules != null)
                {
                    // Initialize the scanner
                    var scanner = new Scanner();

                    List<ScanResult> scanResult =  scanner.ScanString("abcdefgjiklmnoprstuvwxyz", rules);

                    Assert.True(scanResult.Count > 0);
                }
            }
        }

        [Fact]
        public void CheckStringNotMatchTest()
        {
            // Initialize yara context
            using (YaraContext ctx = new YaraContext())
            {
                // Compile yara rules
                CompiledRules rules = null;

                using (var compiler = new Compiler())
                {
                    compiler.AddRuleString("rule foo: bar {strings: $a = \"nml\" condition: $a}");

                    rules = compiler.Compile();
                }

                if (rules != null)
                {
                    // Initialize the scanner
                    var scanner = new Scanner();
                    List<ScanResult> scanResult = scanner.ScanString("abcdefgjiklmnoprstuvwxyz", rules);

                    Assert.True(scanResult.Count == 0);
                }
            }
        }

        [Fact]
        public void CheckMemoryMatchTest()
        {
            // Initialize yara context
            using (YaraContext ctx = new YaraContext())
            {
                // Compile yara rules
                CompiledRules rules = null;

                using (var compiler = new Compiler())
                {
                    compiler.AddRuleString("rule foo: bar {strings: $a = \"lmn\" condition: $a}");

                    rules = compiler.Compile();
                }

                if (rules != null)
                {
                    // Initialize the scanner
                    var scanner = new Scanner();

                    Encoding encoding = Encoding.ASCII;

                    byte[] buffer = encoding.GetBytes("abcdefgjiklmnoprstuvwxyz");

                    List<ScanResult> scanResult = scanner.ScanMemory(ref buffer, rules);

                    Assert.True(scanResult.Count > 0);
                }
            }
        }

        [Fact]
        public void CheckMemoryNotMatchTest()
        {
            // Initialize yara context
            using (YaraContext ctx = new YaraContext())
            {
                // Compile yara rules
                CompiledRules rules = null;

                using (var compiler = new Compiler())
                {
                    compiler.AddRuleString("rule foo: bar {strings: $a = \"nml\" condition: $a}");

                    rules = compiler.Compile();
                }

                if (rules != null)
                {
                    // Initialize the scanner
                    var scanner = new Scanner();

                    Encoding encoding = Encoding.ASCII;

                    byte[] buffer = encoding.GetBytes("abcdefgjiklmnoprstuvwxyz");

                    List<ScanResult> scanResult = scanner.ScanMemory(ref buffer, rules);

                    Assert.True(scanResult.Count == 0);
                }
            }
        }

        [Fact]
        public void CheckIterateRulesTest()
        {
            string ruleText = @"rule foo: bar {
                meta:
                    bool_meta = true
                    int_meta = 10
                    string_meta = ""what a long, drawn-out thing this is!""
                strings:
                    $a = ""nml""
                condition:
                    $a
                }";

            // Initialize yara context
            using (YaraContext ctx = new YaraContext())
            {
                // Compile yara rules
                CompiledRules rules = null;

                using (var compiler = new Compiler())
                {
                    compiler.AddRuleString(ruleText);

                    rules = compiler.Compile();
                }
                System.Threading.Thread.Sleep(2000);

                if (rules != null)
                {
                    var rule = rules.Rules.ToList()[0];
                    Assert.NotEmpty(rules.Rules);
                    Assert.Equal("foo", rule.Identifier);
                    Assert.Equal("bar", rule.Tags[0]);
                    Assert.Equal(true, rule.Metas["bool_meta"]);
                    Assert.Equal((long)10, rule.Metas["int_meta"]);
                    Assert.Equal("what a long, drawn-out thing this is!", rule.Metas["string_meta"]);
                }
            }
        }
    }
}

