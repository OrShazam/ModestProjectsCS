using System;

namespace displayRainbowText
{
    class Program
    {
        static void displayRainbowText(string s)
        {
            int cnt = 0;
            ConsoleColor[] colors = new ConsoleColor[]
            {
                ConsoleColor.Red,ConsoleColor.Yellow,ConsoleColor.Green,ConsoleColor.Cyan,ConsoleColor.Magenta
            };
            foreach (char ch in s)
            {
                if (ch == ' ' || ch == '\t' || ch == '\n')
                {
                    Console.Write(ch); continue;
                }
                Console.ForegroundColor = colors[(cnt++) % 5];
                Console.Write(ch);
            }
            Console.WriteLine();
            Console.ResetColor();
        }
        static void Main(string[] args)
        {
            displayRainbowText("Hello World!");
        }
    }
}
