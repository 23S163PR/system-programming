using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace multi_treading_max
{
    static class Program
    {
        static void Main(string[] args)
        {
             
            var arr = new List<int>();
            var rnd = new Random();
            for (var i = 0; i < 500000; i++)
            {
                arr.Add(rnd.Next(100));
            }

            int max = 0;
            Parallel.ForEach(arr, i =>
            {
               Maximum(ref max, i);
            });

            Console.WriteLine("\nmax in arr - {0}", arr.Max());
            Console.WriteLine("max in multimax - {0}", max);
        }

        static void Maximum(ref int max, int value)
        {
            if (max < value)
            {
                Interlocked.Exchange(ref max, value);
            }
        }
    }
}
