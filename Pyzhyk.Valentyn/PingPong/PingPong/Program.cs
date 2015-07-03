using System;
using System.Threading;

namespace PingPong
{
    class Program
    {
        static void Main()
        {
            var isPing = true;
            var isPong = false;

            var ping = new Thread(() =>
            {
                while (true)
                {
                    if (isPing)
                    {
                        Console.WriteLine("Ping");
                        Thread.Sleep(1000);
                        isPong = true;
                        isPing = false;
                    }
                }

            }) {IsBackground = false};

            var pong = new Thread(() =>
            {
                while (true)
                {
                    if (isPong)
                    {
                        Console.WriteLine("Pong");
                        Thread.Sleep(1000);
                        isPing = true;
                        isPong = false;
                    }
                }
            }) {IsBackground = false};

            Console.WriteLine("Press any key to start Ping - Pong");
            Console.ReadKey();

            pong.Start();
            ping.Start();

            while (true)
            {
                Thread.Sleep(100);
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
