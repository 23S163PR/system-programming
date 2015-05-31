using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sorts
{
	struct Result
	{
		public string Name;
		public double Time;
	}

	internal class Program
	{
		private static void Main(string[] args)
		{
			var bubble = GetRandomArray(10000); // 10000 - size of int array
			var quick = (int[])bubble.Clone();
			var selection = (int[])bubble.Clone();
			var merge = (int[])bubble.Clone();

			var taskFactory = new TaskFactory(
				TaskCreationOptions.AttachedToParent,
				TaskContinuationOptions.None);

			var tasks = new[]
			{
				taskFactory.StartNew(() => GetRuntime(() => Sorts.BubleSort(bubble))),
				taskFactory.StartNew(() => GetRuntime(() => Sorts.QuickSort(quick,0,quick.Length-1))),
				taskFactory.StartNew(() => GetRuntime(() => Sorts.SelectionSort(selection))),
				taskFactory.StartNew(() => GetRuntime(() => Sorts.MergeSort(merge,0,merge.Length-1)))
			};

            foreach (var res in tasks.OrderBy(res => res.Result.Time))
			{
				Console.WriteLine("{0, -20} : {1, -10} Miliseconds", res.Result.Name, res.Result.Time);
			}
		}

		static Result GetRuntime(Func<string> sortMethod)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			var sortMethodName = sortMethod();
			stopWatch.Stop();

			return new Result
			{
				Name = sortMethodName,
				Time = stopWatch.Elapsed.TotalMilliseconds
			};
		}

		private static int[] GetRandomArray(int size)
		{
			var random = new Random();
			var array = new int[size];
			for (var i = 0; i < array.Length; i++)
			{
				array[i] = random.Next(0, 1000);
			}
			return array;
		}
	}
}
