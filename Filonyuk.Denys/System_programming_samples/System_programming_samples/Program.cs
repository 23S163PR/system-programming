using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
namespace System_programming_samples
{
    class Program
    {
        public static long Max;
        
        static void Main(string[] args)
        {
            const int size = 100;
            Console.WriteLine("Max val:{0}",Max);
            Random rnd = new Random();
            System.Collections.Concurrent.ConcurrentBag<int> Digits = new ConcurrentBag<int>();
            
            
            Parallel.For(0, size, i =>
            {
                Digits.Add(rnd.Next(0, 100));

            });
            
            Console.WriteLine("Generated numbers");
            foreach (var ob in Digits)
            {
                Console.WriteLine(ob);
            }
            Parallel.For(0, size, i =>
                {
                    Maximum(ref Max, Digits.ElementAt(i));
                    Console.WriteLine(" ThreadID: {0}  i:{1} Max:{2}",Thread.CurrentThread.ManagedThreadId,i, Max);   
                });
            Console.WriteLine("Max from Generated Digits:{0}", Max);
            //Console.WriteLine(Max);
           
        }

        public static void Maximum(ref long Max, int value)
        {

            if (Interlocked.Read(ref Max) < value)
            {
                Interlocked.Exchange(ref Max, value);
            }
            else
            {
                Interlocked.Exchange(ref Max, Interlocked.Read(ref Max));
            }
        }
    }

    
}
