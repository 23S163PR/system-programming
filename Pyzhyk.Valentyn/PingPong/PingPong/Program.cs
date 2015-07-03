using System;
using System.Threading;

namespace PingPong
{
    class Program
    {
        static void Main()
        {
            var flag = true;
            //var isPong = false;

            var ping = new Thread(() =>
            {
                while (true)
                {
                    if (flag)
                    {
                        Console.WriteLine("Ping");
                        flag = false;
                    }
                }

            }) {IsBackground = false};

            var pong = new Thread(() =>
            {
                while (true)
                {
                    if (flag == false)
                    {
                        Console.WriteLine("Pong");
                        flag = true;
                    }
                }
            }) {IsBackground = false};

            Console.WriteLine("Press any key to start Ping - Pong");
            Console.ReadKey();

            pong.Start();
            ping.Start();

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                {
                    pong.Abort();
                    ping.Abort();
                    break;
                }
            }
            Console.WriteLine("End programm");
            Console.ReadKey();
        }
    }
}
