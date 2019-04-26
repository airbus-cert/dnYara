using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnYara;
using dnYara.Interop;

namespace YaraInteractive
{
    internal static class CmdHandler
    {
        public static List<string> yaras = new List<string>();
        public static List<string> samples = new List<string>();
        public static CompiledRules rules;

        internal static bool ExecuteCmd(string command)
        {
            bool isManagedCmd = false;
            string[] commands = command.Split(' ');

            string cmdName = commands.First();
            string[] args = commands.Skip(1).ToArray();

            switch (cmdName)
            {
                case "exit":
                    Environment.Exit(0);
                    break;
                case "clear":
                    Console.Clear();
                    isManagedCmd = true;
                    break;
                case "yadd":
                    isManagedCmd = true;
                    CmdAddRules(args);
                    break;
                case "sadd":
                    isManagedCmd = true;
                    CmdAddSamples(args);
                    break;
                case "ycompile":
                    isManagedCmd = true;

                    using (var compiler = new Compiler())
                    {
                        foreach (var yara in yaras.Distinct())
                        {
                            var err = ScanHelper.CheckRule(yara);

                            if (err == YARA_ERROR.SUCCESS)
                            {
                                try
                                {
                                    compiler.AddRuleFile(yara);
                                    Console.WriteLine($":Added {yara}");
                                } catch (Exception e)
                                {
                                    Console.WriteLine($"!Exception adding \"{yara}\": {e.Message}");
                                }
                            }
                            else
                                Console.WriteLine($"!Exception adding \"{yara}\": {err}");
                        }

                        try
                        {
                            rules = compiler.Compile();
                            Console.WriteLine($"* Compiled");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"!Exception compiling rules: {e.Message}");
                        }
                    }

                    break;
                case "run":
                    isManagedCmd = true;
                    CmdRun();
                    break;
            }


            return isManagedCmd;
        }

        private static void CmdRun()
        {
            var scanner = new Scanner();

            foreach (var sample in samples)
            {
                if (File.Exists(sample))
                {
                    ScanFile(scanner, sample);
                }
                else
                {
                    if (Directory.Exists(sample))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(sample);

                        foreach (FileInfo fi in dirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                            ScanFile(scanner, fi.FullName);
                    }
                }
            }

        }

        private static void CmdAddRules(string[] args)
        {
            foreach (var rule in args)
            {
                yaras.Add(rule);

                Console.WriteLine($":ok rules: {rule}");
            }
        }

        private static void CmdAddSamples(string[] args)
        {
            foreach (var sample in args)
            {
                samples.Add(sample);

                Console.WriteLine($":ok sample: {sample}");
            }
        }

        public static void ScanFile(Scanner scanner, string filename)
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
