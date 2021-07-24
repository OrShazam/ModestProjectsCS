using System;

namespace getPass
{
// python's get pass for cs
    class Program
    {
        static string GetPass(string message)
        {
            string pass = "";
            Console.Write(message);
            Console.ForegroundColor = Console.BackgroundColor;
            char key;
            while ((key = Console.ReadKey().KeyChar) != '\r' && key != '\n')
            {
                if ((int)key == 8)
                {
                    Console.CursorLeft += 1;
                    if (pass.Length != 0)
                        pass = pass.Substring(0, pass.Length - 1);
                    continue;
                }

                else if ((int)key > 31)
                    pass += key;
                if (Console.CursorLeft > 0)
                    Console.CursorLeft -= 1;
            }
            Console.ResetColor();
            Console.WriteLine();
            return pass;
        }
        static void Main(string[] args)
        {
            string pass = GetPass("GIMME CREDS: ");
            Console.WriteLine("YOUR SECRET CREDS THAT SHALL NEVER BE PRINTED: " + pass);
            Console.ReadKey();
        }
    }