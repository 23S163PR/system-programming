using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sort
{
    public class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            var bubble = new int[1000];

            for (int i = 0; i < bubble.Length; i++)
            {
                bubble[i] = rnd.Next(0, 40);
            }

            var quick = (int[])bubble.Clone();
            var selection = (int[])bubble.Clone();
            var merge = (int[])bubble.Clone();


            var taskFactory = new TaskFactory(
				TaskCreationOptions.AttachedToParent,
				TaskContinuationOptions.None);

			var tasks = new[]
			{
				taskFactory.StartNew(() => GetRuntime(() => Sorts.BubbleSort(bubble))),
				taskFactory.StartNew(() => GetRuntime(() => Sorts.QuickSort(quick,0,quick.Length-1))),
				taskFactory.StartNew(() => GetRuntime(() => Sorts.SelectionSort(selection))),
				taskFactory.StartNew(() => GetRuntime(() => Sorts.MergeSort(merge,0,merge.Length-1)))
			};

			foreach (var time in tasks)
			{
				Console.WriteLine("{0, -10} Miliseconds \n", time.Result);
			}
        }

        static double GetRuntime(Func<int[]> sortMethod)
		{
			var stopWotch = new Stopwatch();
			stopWotch.Start();
			sortMethod();
			stopWotch.Stop();
			return stopWotch.Elapsed.TotalMilliseconds;
		}   
    }
}


