using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maximum
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const int size = 10000;
            var max = 0;

            Parallel.For(0, size, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount*60 }, x =>
            {
                Maximum(ref max, x);
            });

            Console.WriteLine(max);
        }

        // потокобезопасна
        private static object sync = new object();
        public static void Maximum(ref int target, int value)
        {
            lock (sync)
            {
                if (target < value)
                {
                    Interlocked.Exchange(ref target, value);
                }
            }
        }
    }
}
