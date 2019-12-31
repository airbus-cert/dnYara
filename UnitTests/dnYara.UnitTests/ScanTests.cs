using System.Collections.Generic;
using System.Text;
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
                System.Threading.Thread.Sleep(2000);

                if (rules != null)
                {
                    Assert.NotEmpty(rules.Rules);
                    var rule = rules.Rules[0];
                    System.Console.WriteLine($"rule {rule.Identifier}, tags: {rule.Tags}");
                    Assert.Equal("foo", rule.Identifier);
                    Assert.Equal("bar", rule.Tags[0]);
                }
            }
        }
    }
    class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("HEllo");
            new ScanTests().CheckIterateRulesTest();
        }
    }
}

