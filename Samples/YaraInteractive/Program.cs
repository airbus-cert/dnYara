using System;
using dnYara;

namespace YaraInteractive
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("# Welcome to Yara Interactive Console...");

            while (true)
            {
                Console.Write("> ");

                string command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                    continue;

                bool isManagedCmd = CmdHandler.ExecuteCmd(command);

                if (!isManagedCmd)
                    Console.WriteLine(":Err: Unknown command...");
            }
        }
    }
}
