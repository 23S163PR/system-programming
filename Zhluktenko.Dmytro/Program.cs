using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    class Program
    {
        public static void Ping()
        {
            Console.WriteLine("Ping ");
        }
        public static void Pong()
        {
            Console.WriteLine("Pong ");
        }
        static void Main(string[] args)
        {

           
             while (true)
            {
            Task t1 = Task.Factory.StartNew(() => Ping());
            t1.Wait();
            Task t2 = Task.Factory.StartNew(() => Pong());
            t2.Wait();

            if (Console.KeyAvailable == true)
            {
                try
                {
                    t1.Dispose();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Task 1 is {}", t1.Status);
                }
                try
                {
                    t2.Dispose();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Task 1 is {}", t1.Status);
                }
                break;
            }
                 
            }
            
        }
       
    }
}
