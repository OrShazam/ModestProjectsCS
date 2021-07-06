using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
// a decryptor for a vigenere-encrypted message (without a given key), written in C#
namespace ConsoleAppSomething
{
    class VigenereDecryptor
    {
        static string tryDecrypt(string key, string text)
        {
            string decryptedText = "";    
            for (int i = 0; i < text.Length; i++)
            {
                int CharAscii = (int)text[i];
                int decryptedCharAscii = CharAscii - ((int)key[i % key.Length] - 65);
                if (decryptedCharAscii < 65)
                    decryptedCharAscii =  90 - (64 - decryptedCharAscii);
                decryptedText += (char)decryptedCharAscii;
            }
            return decryptedText;
        }
        static void tryingBruteForce(int keyLen, string text)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; string tempKey = ""; int count = 0;
            for (int i = 0; i < Math.Pow(26, keyLen); i++)
            {
                tempKey += chars[i % 26];
                for(int j = 1; j < keyLen; j++)
                {
                    tempKey += chars[(i / (int)Math.Pow(26,j)) % 26];
                }
                string possibleText = tryDecrypt(tempKey, text);
                if ( coincidenceCounting(possibleText) < 1.8 && coincidenceCounting(possibleText) > 1.55)
                {
                    count++;
                    Console.WriteLine($"Possible decrypted text #{count} : {possibleText}");
                }
                tempKey = "";
                
            }

        }
        
        static void decrpytVigenere(string text)
        {
            //after texting the coincideCounting function, I think that somewhere between bigger than 1.55 and smaller than 1.8 is reasonable for testing
            // we can check how much a text is legitimate, we can find the key length, all there is left is to bruteforce possible keys
            Console.WriteLine("This is going to take sometime...");
            int keyLen = probableFreq(text);
            tryingBruteForce(keyLen, text);

        }
        static double coincidenceCounting(string text)
        {
            // using Friedman's equation, the coincidence for english text is at around 1.73
           
            int[] occurenceCounter = new int[26];
            for(int i = 0; i < text.Length;  i++)
            {
                occurenceCounter[(int)text[i] - 65]++;
            }
            double IC = 0;
            for(int i = 0; i < occurenceCounter.Length; i++)
            {
                IC += (double)occurenceCounter[i] / text.Length * (double)(occurenceCounter[i] - 1) / (text.Length - 1);
            }

            return IC * 26;
        }
        static int indexForContained(string s1, string s2)
        {
            for (int i = 0; i < s1.Length - s2.Length; i++)
            {
                if(s1.Substring(i, s2.Length) == s2)
                {
                    return i;
                }
            }
            return -1; // returns -1 if the s1 didn't contain s2
        }
       
        static int probableFreq(string text)
        {
            //using Babbage's idea that the most common factorial for spacing for repetition in the text would indicate the key's length
            //the function is accurate enough if we only check through 3 characters long repetitions
            int[] probableFreqCounters = new int[21]; // the count is stored in the index of the factorial fo the spacing, 
            // the function is accurate enough if we only check for the factorials till 20, the key can be longer than 20 but it's unlikely
            int maxCount = 0; int mostFrequentSpacing = 0;
            string bfr = text.Substring(0, 3);
            for (int i = 3; i < text.Length - 3; i++)
            {
                int index = indexForContained(bfr, text.Substring(i, 3));
                if (index != -1)
                {
                    int spacing = i - index;
                    // because I don't have the mentality to make a factorials function I'll just hardcore the check here
                    for (int j = 2; j < 21; j++)
                    {
                        if (spacing % j == 0)
                        {
                            probableFreqCounters[j]++;
                        }

                    }

                    
                }
                bfr += text[i];

            }
            for (int i = 2; i < 21; i++)
            {
                if (probableFreqCounters[i] > maxCount)
                {
                    maxCount = probableFreqCounters[i];
                    mostFrequentSpacing = i;
                }
            }
            return mostFrequentSpacing;
        }

        
    }
}