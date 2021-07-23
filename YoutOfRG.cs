using System;
using System.Threading;
// just run it and you'll get it
namespace code
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleColor[] colors = new ConsoleColor[]
            {
                ConsoleColor.Red,ConsoleColor.Green
            };
            int count = 0;
            while (true)
            {
                Console.ForegroundColor = colors[count++];
                Console.Write($"TickCount: {Environment.TickCount}");
                Console.CursorLeft = 0;
                Thread.Sleep(1);
                if (count == 2)
                    count -= 2;
            }
            Console.ReadKey();
        }
    }
}