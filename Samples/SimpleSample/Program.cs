using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnYara;

namespace SimpleSample
{
    class Program
    {

        static void Main(string[] args)
        {
            // Get list of yara rules
            string[] ruleFiles = Directory.GetFiles(@"e:\yara-db\rules\", "*.yara", SearchOption.AllDirectories)
                .ToArray();

            // Get list of samples to check
            string[] samples = new[]
            {
                @"e:\malware-samples\", // directory
                @"e:\speficic-samples\sample1.exe" // file
            };

            // Initialize yara context
            using (YaraContext ctx = new YaraContext())
            {
                // Compile list of yara rules
                CompiledRules rules = null;
                using (var compiler = new Compiler())
                {
                    foreach (var yara in ruleFiles)
                    {
                        compiler.AddRuleFile(yara);
                    }

                    rules = compiler.Compile();

                    Console.WriteLine($"* Compiled");
                }

                if (rules != null)
                {
                    // Initialize the scanner
                    var scanner = new Scanner();

                    // Go through all samples
                    foreach (var sample in samples)
                    {
                        // If item is file, scan the file
                        if (File.Exists(sample))
                        {
                            ScanFile(scanner, sample, rules);
                        }
                        // If item is directory, scan the directory
                        else
                        {
                            if (Directory.Exists(sample))
                            {
                                DirectoryInfo dirInfo = new DirectoryInfo(sample);

                                foreach (FileInfo fi in dirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                                    ScanFile(scanner, fi.FullName, rules);
                            }
                        }
                    }

                }

            }
        }

        public static void ScanFile(Scanner scanner, string filename, CompiledRules rules)
        {
            List<ScanResult> scanResults = scanner.ScanFile(filename, rules);

            foreach (ScanResult scanResult in scanResults)
            {
                string id = scanResult.MatchingRule.Identifier;

                if (scanResult.Matches.Count == 1)
                {
                    Console.WriteLine(
                        $"* Match found : \"{filename}\" for rule \"{id}.{scanResult.Matches.First().Key}\"");
                }
                else
                {
                    Console.WriteLine($"* Match found : \"{filename}\" for rule \"{id}\":");

                    foreach (var vd in scanResult.Matches)
                    {
                        Console.WriteLine($"   > Rule : \"{vd.Key}\"");
                    }
                }
            }
        }
    }
}
