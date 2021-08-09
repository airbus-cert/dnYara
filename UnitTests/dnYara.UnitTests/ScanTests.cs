using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using System;

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

                    List<ScanResult> scanResult = scanner.ScanString("abcdefgjiklmnoprstuvwxyz", rules);

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

        [Fact]
        public void CheckSaveLoadRuleTest()
        {
            // Initialize yara context
            using (YaraContext ctx = new YaraContext())
            {
                using (var compiler = new Compiler())
                {

                    compiler.AddRuleString("rule foo: bar {strings: $a = \"lmn\" condition: $a}");
                    CompiledRules compiledRules = compiler.Compile();
                    Assert.True(compiledRules.RuleCount == 1);

                    Encoding encoding = Encoding.ASCII;
                    byte[] buffer = encoding.GetBytes("abcdefgjiklmnoprstuvwxyz");

                    // Initialize the scanner
                    var scanner = new Scanner();

                    List<ScanResult> compiledScanResults = scanner.ScanMemory(ref buffer, compiledRules);
                    Assert.True(compiledScanResults.Count == 1);
                    Assert.Equal("foo", compiledScanResults[0].MatchingRule.Identifier);


                    //save the rule to disk
                    string tempfile = System.IO.Path.GetTempFileName();
                    bool saved = compiledRules.Save(tempfile);
                    Assert.True(saved);

                    //load the saved rule to a new ruleset
                    CompiledRules loadedRules = new CompiledRules(tempfile);

                    List<ScanResult> loadedScanResults = scanner.ScanMemory(ref buffer, loadedRules);

                    Assert.True(loadedScanResults.Count == 1);
                    Assert.Equal("foo", loadedScanResults[0].MatchingRule.Identifier);

                    System.IO.File.Delete(tempfile);
                }
            }
        }


        [Fact]
        public void CheckExternalVariableRuleTest()
        {
            // Initialize yara context
            using (YaraContext ctx = new YaraContext())
            {
                using (var compiler = new Compiler())
                {
                    //must declare this or the compiler will complain it doesn't exist when a rule references it
                    compiler.DeclareExternalStringVariable("filename");

                    //declare rule with an external variable available
                    compiler.AddRuleString("rule foo: bar {strings: $a = \"lmn\" condition: $a and filename matches /\\.txt$/is}");
                    CompiledRules compiledRules = compiler.Compile();

                    Assert.True(compiledRules.RuleCount == 1);

                    Encoding encoding = Encoding.ASCII;
                    byte[] buffer = encoding.GetBytes("abcdefgjiklmnoprstuvwxyz");

                    // Initialize a customscanner we can add variables to
                    var scanner = new CustomScanner(compiledRules);

                    ExternalVariables externalVariables = new ExternalVariables();
                    externalVariables.StringVariables.Add("filename", "Alphabet.txt");

                    List<ScanResult> compiledScanResults = scanner.ScanMemory(ref buffer, externalVariables);

                    Assert.True(compiledScanResults.Count == 1);
                    Assert.Equal("foo", compiledScanResults[0].MatchingRule.Identifier);

                    //release before falling out of the yara context
                    scanner.Release();                    
                }



            }
        }
    }
}

