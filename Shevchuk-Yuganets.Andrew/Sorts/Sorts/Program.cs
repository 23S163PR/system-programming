using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sorts
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var bubble = GetRandomArray(1000); // 10000 - size of int array
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

			foreach (var time in tasks)
			{
				Console.WriteLine("\t{0, -10} Miliseconds \n", time.Result);
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

		private static void Print(int[] array)
		{
			foreach (var number in array)
			{
				Console.Write("{0} ", number);
			}
		}
	}
}
