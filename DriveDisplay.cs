using System;
using System.IO;
namespace environmentEqualsHood
{
// a simple program to list information about logical drives (fancy)
    class Program
    {
        static void RootDirectoryListing(DirectoryInfo root)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\tVERBOSE: ROOT DIRECTORY LISTING");
            int count = 0;
            Console.WriteLine("\t\tDIRECTORIES:");
            Console.Write("\t");
            foreach (DirectoryInfo dir in root.GetDirectories())
            {
                Console.Write("\t\\" + dir.Name);
                if ((count++) == 10) { Console.WriteLine(); Console.Write("\t"); count = 0; }
            }
            Console.WriteLine("\t\tFILES:");
            foreach(FileInfo file in root.GetFiles())
            {
                if (Path.GetExtension(file.FullName) == ".exe")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\t" + file.Name);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                Console.Write("\t" + file.Name);
                if ((count++) == 10) { Console.WriteLine(); Console.Write("\t"); count = 0; }
            }
            Console.ResetColor();
        }
        static void DriveStatus(bool isReady)
        {
            if (isReady)
            {
                Console.Write("\t[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("*");
                Console.ResetColor();
                Console.Write("] READY");
            }
            else
            {
                Console.Write("\t[] NOT READY");
            }
            Console.WriteLine();
        }
        static void DriveTitle(string driveName, int index)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\t#{index} " + driveName);
            Console.ResetColor();
        }
        static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\t[WARNING]: " + message);
            Console.ResetColor();
        }
        static void Main(string[] args)
        {
            int index = 1;
            bool verbose = false;
            if(args.Length > 0)
            {
                if (args[0] == "-v")
                    verbose = true;
            }
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                DriveTitle(drive.Name, (index++));
                try
                {
                    // string tab = String.
                    double sizeInGB = drive.TotalSize / 1073741824.0;
                    double freeInGB = drive.TotalFreeSpace / 1073741824.0;
                    Console.Write("\tTOTAL SPACE: {0:0.0}GB\t",sizeInGB);
                    Console.Write("USED: {0:0.0}GB\t", sizeInGB - freeInGB);
                    Console.Write("FREE: {0:0.0}GB\n", freeInGB);
                    Console.Write($"\tUSAGE: {drive.VolumeLabel}");
                    Console.Write($"\tFILESYSTEM: {drive.DriveFormat}");
                    Console.Write($"\t({drive.DriveType.ToString().ToUpper()})\n");
                    DriveStatus(drive.IsReady);      
                    if (verbose)
                    {
                        RootDirectoryListing(drive.RootDirectory);
                        // hopefully more stuff later on
                    }
                }
                catch
                {
                    Warning($"Can't retrieve data about drive {drive.Name}");
                    // it seems like bad practice but mostly it's either all the data is unavailable or it's all available
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}