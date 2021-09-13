using System;
using System.IO;
using System.Threading;

namespace Singleton
{
	// making sure the program runs only one time using named mutex
    class Program
    {
        static void DoStuff()
        {

        }
        static string GetIdentifier()
        {
            string currPath = AppDomain.CurrentDomain.BaseDirectory;
            string idPath = Path.Combine(currPath, "id.txt");
            if (!File.Exists(idPath))
                CreateIdentifier(idPath);

            return File.ReadAllText(idPath);

        }
        static void CreateIdentifier(string idPath)
        {
            string id = Guid.NewGuid().ToString();
            string programName = AppDomain.CurrentDomain.FriendlyName;
            Console.WriteLine
                ($"Using Identifier {id} for program {programName}...");
            Console.WriteLine("Saving identifier as id.txt...");
            File.WriteAllText(idPath, id);
        }

        static void Main(string[] args)
        {
            Mutex mut;
            string id = GetIdentifier();
            bool mutExists = Mutex.TryOpenExisting(id, out mut);
            if (mutExists)
            {
                mut.Close(); // I think that's should be here?
                Console.WriteLine("Program is already running...");
                Console.WriteLine("Exiting...");
                Thread.Sleep(1000);
                Environment.Exit(1);
            }
            else
            {
                bool initThreadOwnsFirst = true;
                mut = new Mutex(initThreadOwnsFirst, id);
                DoStuff();
                mut.Dispose();
            }


            Console.ReadKey();
        }
    }
}
