using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Sorts
{
	public struct Result
	{
		public string Name;
		public double Time;
	}

	internal class Program
	{
		private static void Main(string[] args)
		{
			Sort();
			Console.ReadLine();
		}

		async static void Sort()
		{
			var bubble = GetRandomArray(10000); // 10000 - size of int array
			var quick = (int[])bubble.Clone();
			var selection = (int[])bubble.Clone();
			var merge = (int[])bubble.Clone();

			var taskFactory = new TaskFactory();
			var tasks = new[]
			{
				taskFactory.StartNew(() => GetRuntime(() => Sorts.BubleSort(bubble))),
				taskFactory.StartNew(() => GetRuntime(() => Sorts.QuickSort(quick,0,quick.Length-1))),
				taskFactory.StartNew(() => GetRuntime(() => Sorts.SelectionSort(selection))),
				taskFactory.StartNew(() => GetRuntime(() => Sorts.MergeSort(merge,0,merge.Length-1)))
			};
			await Task.WhenAny(tasks)
				.ContinueWith(t =>
				{
					Console.WriteLine("The champion is: {0} with {1}ms time!", 
						t.Result.Result.Name, t.Result.Result.Time);
				});

			await Task.WhenAll(tasks)
				.ContinueWith(t =>
				{
					foreach (var time in t.Result)
					{
						Graphics.TotalTime += time.Time;
					}

					foreach (var time in t.Result)
					{
						Graphics.Print(time);
					}
				});
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

	//
	public static class Graphics
	{
		private static readonly Random Rand = new Random();

		public static double TotalTime { get; set; }

		private static int _xStartPos = 10;
		private static int _yStartPos = 50;
		private static int _columnWidth = 3;
		private static int _distanceBetweenColumns = 20;

		private static readonly List<int> ColorList = new List<int>();

		public static void Print(Result result)
		{
			Console.ForegroundColor = GetRandomColor();

			double value = (result.Time / TotalTime) * 100;

			for (int y = 0; y < ((int)value / 2) + 1; y++)
			{
				for (int i = 0; i < _columnWidth; i++)
				{
					Console.SetCursorPosition(_xStartPos + i, _yStartPos - y);
					Console.Write(Encoding.GetEncoding(437 /* █ */).GetChars(new byte[] { 219 })[0]);
				}
			}

			// sort percentage line
			Console.SetCursorPosition(_xStartPos - 1, _yStartPos + 2);
			Console.WriteLine("{0}%", value.ToString("0.00"));

			// sort name line
			Console.SetCursorPosition(_xStartPos - (result.Name.Length / 2) + 1, _yStartPos + 3);
			Console.WriteLine(result.Name);

			_xStartPos += _distanceBetweenColumns;
		}

		private static ConsoleColor GetRandomColor()
		{
			ConsoleColor color;
			int numberColor;

			do
			{
				numberColor = Rand.Next(9, 15);
			} while (ColorList.Contains(numberColor));

			ColorList.Add(numberColor);

			Enum.TryParse(numberColor.ToString(), out color);

			return color;
		}
	}
}
