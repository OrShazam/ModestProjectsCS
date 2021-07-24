using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace OffsetFinder
{
    // simple offset finder that simply doesn't work
    // I personally blame stack protections techniques and my poor coding skills
    class Program
    {
        static readonly int GIVEUPCOUNT = 10000; //IDK?
        static void Main(string[] args)
        {
            getpath:
            Console.WriteLine("gimme path for executable");
            string path = Console.ReadLine();
            if (!File.Exists(path)){
                Console.WriteLine("[!] There's no exectuable in the given location.");
                goto getpath;
            }
            if (Path.GetExtension(path) != ".exe")
            {
                Console.WriteLine("[!] File is not an executable.");
                goto getpath;
            }
            int waitSeconds = 3;
            Process runExe = new Process();
            int count = 1;
            string testData = "";
            runExe.StartInfo = new ProcessStartInfo(path)
            {
                Arguments = testData,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            bool skipScan = false;
            while (true)
            {
                if (count == GIVEUPCOUNT)
                {
                    Console.WriteLine($"After {count} tries, I'm giving up.");
                    Environment.Exit(-1);
                }
                skipScan = false;
                testData += new String('A', (count++) * 8);
                runExe.StartInfo.Arguments = testData;
                Console.CursorLeft = 0;
                Console.Write($"Trying offest: {testData.Length} bytes");
                runExe.Start();
                runExe.WaitForExit(waitSeconds * 1000);
                if (!runExe.HasExited)
                {
                    runExe.Kill();
                    Console.CursorLeft = 0;
                    Console.Write($"ERROR: timed out, increasing waiting time to {waitSeconds} seconds");
                    testData = testData.Substring(0, testData.Length - 8); //cause current try wasn't actually tried
                    waitSeconds++;
                    skipScan = true;
                }
                if (!skipScan)
                {
                    string output;
                    while ((output = runExe.StandardOutput.ReadLine()) != null)
                    {
                        if (output.Contains("Segmentation fault") || output.Contains("SIGSEGV"))
                        {
                            Console.WriteLine();
                            Console.WriteLine($"[+] Found return address offset after {testData.Length} bytes");
                            Environment.Exit(0);
                        }
                    }
                }



                Thread.Sleep(10);
            }
            

        }
    }
}
