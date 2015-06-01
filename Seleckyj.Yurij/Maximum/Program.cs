using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maximum
{
    class Program
    {
        static void Main(string[] args)
        {
           
        }

        public static void Maximum(ref int target , int value)
        {
            Interlocked.Exchange(ref target, target < value ? value : target);
        }
    }
}
