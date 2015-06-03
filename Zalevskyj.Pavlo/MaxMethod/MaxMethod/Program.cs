using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaxMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> arr = new List<int>();
            Random rnd = new Random();
            for (var i = 0; i < 1000; i++)
            {
                arr.Add(rnd.Next(100000));
            }

            var max = 0;
            Parallel.For(0, arr.Count, i =>
            {
                Maximum(ref max, arr[i]);
            });

            Console.WriteLine("max in arr - {0}", arr.Max());
            Console.WriteLine("max in Maximum method - {0}", max);
        }

        static void Maximum(ref int max, int value)
        {
            if (Volatile.Read(ref max) < Volatile.Read(ref value))
            {
                Volatile.Write(ref max, value);
            }
        }
    }
}
