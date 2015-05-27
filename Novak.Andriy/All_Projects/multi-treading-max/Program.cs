using System;
using System.Collections.Generic;
using System.Linq;
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
                arr.Add(rnd.Next(100) +i*2);
            }

            int max = 0;
            Parallel.ForEach(arr, i =>
            {
                MultiMax.Max(ref max, i);
            });

            Console.WriteLine("\nmax in arr - {0}", arr.Max());
            Console.WriteLine("max in multimax - {0}", max);
        }
    }
}
