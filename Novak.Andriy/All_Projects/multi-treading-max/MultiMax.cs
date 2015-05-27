using System.Threading;

namespace multi_treading_max
{
    static class MultiMax
    {
        public static void Max(ref int max, int value)
        {
            if (max < value)
            {
                Interlocked.Exchange(ref max, value);
            }
        }
    }
}
