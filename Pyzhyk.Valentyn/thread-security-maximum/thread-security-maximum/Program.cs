using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace thread_security_maximum
{
    class Program
    {
        
        static void Main(string[] args)
        {
            int MaxNumber = 0;
            Random random = new Random();
            int[] arr = new int[]{15,20};
            Parallel.For(0, 1000000, ctr =>
            {
                ThreadSecurity.Maximum(ref MaxNumber, arr[random.Next(0,2)]);
            });
            Console.WriteLine("The end of writing! max: {0}", MaxNumber);
            
        }
    }
    public class ThreadSecurity
    {
        public static void Maximum(ref int target, int value)
        {
            Interlocked.Exchange(ref target, target < value ? value : target);

            //Interlocked.Exchange(ref target, (Interlocked.Exchange(ref target, target) < value ? value : target));
        }
    }
}
