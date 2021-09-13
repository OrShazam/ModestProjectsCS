using System;

namespace ColorGrep
{
	// color greps text and subtext, maybe more fitting for a powershell script...
    class Program
    {
        static void coloredPrint(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        static void colorGrep(string text, string subtext, ConsoleColor color)
        {
            int subLen = subtext.Length;
            int textLen = text.Length;
            for(int i = 0; i < textLen; i++)
            {
                if (text[i] == subtext[0])
                {
                    // if not don't even bother compare substring
                    if (textLen - i >= subLen && text.Substring(i, subLen) == subtext)
                    {
                        coloredPrint(subtext, color);
                        i += subLen - 1;
                    }
                    else
                        Console.Write(text[i]);
                }
                else
                    Console.Write(text[i]);
            }

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Write a string:");
            string text = Console.ReadLine();
            Console.WriteLine("And now a substring to color grep:");
            string subtext = Console.ReadLine();
            // get options for color if it was enterprise 
            ConsoleColor yellow = ConsoleColor.Yellow;
            colorGrep(text, subtext, yellow);
            Console.ReadKey();
        }
    }
}
