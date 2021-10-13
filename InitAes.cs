using System;
using System.Security.Cryptography;

namespace InitCrypto
{
    class Program
    { 
        static void printWithoutDash(string key, string IV)
        {
            Console.Write("KEY: 0x");
            for (int i = 0; i < key.Length; i++)
            {
                if ((i + 1) % 3 == 0)
                    continue;
                Console.Write(key[i]);
            }
            Console.WriteLine();
            Console.Write("IV: 0x");
            for (int i = 0; i < IV.Length; i++)
            {
                if ((i + 1) % 3 == 0)
                    continue;
                Console.Write(IV[i]);
            }
        }
        static void Main(string[] args)
        {

            using (Aes aes = Aes.Create())
            {
                string key = BitConverter.ToString(aes.Key);
                string IV = BitConverter.ToString(aes.IV);
                printWithoutDash(key, IV);
            }
            Console.ReadKey();  
        }
    }
}
