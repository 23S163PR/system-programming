using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SortingArray
{
    static class Program
    {
        struct TestResult
        {
            public string Name { get;set; }
            public double Time { get; set; }
        }

        static void Main(string[] args)
        {

            var bubleArr = RandArr(10000, new Random());
            var quickArr = (int[])bubleArr.Clone();
            var selectionArr = (int[])bubleArr.Clone();
            var mergArr = (int[])bubleArr.Clone();
            var task = new TaskFactory(
                TaskCreationOptions.AttachedToParent,
                TaskContinuationOptions.None);

            var tasks = new[]
            {
                task.StartNew(() => GetTime(() => SortArray.BubleSort(bubleArr)))
                ,task.StartNew(() => GetTime(() => SortArray.QuickSort(quickArr,0,quickArr.Length-1)))
                ,task.StartNew(() => GetTime(() => SortArray.SelectionSort(selectionArr)))
                ,task.StartNew(() => GetTime(() => SortArray.MergeSort(mergArr,0,mergArr.Length-1)))   
            };

            task.ContinueWhenAny(tasks,
                first => { Console.WriteLine("\nFirst - {0}\t{1} Miliseconds\n", first.Result.Name, first.Result.Time); });
            
            foreach (var t in tasks)
            {
                Console.WriteLine("\nMethod - {0}\t{1} Miliseconds", t.Result.Name, t.Result.Time);
            }  
        }

        static TestResult  GetTime(Func<string> func)
        {
            var stopWotch = new Stopwatch();
            stopWotch.Start();
            var str = func();
            stopWotch.Stop();
            return new TestResult
            {
                Time = stopWotch.Elapsed.TotalMilliseconds,
                Name = str
            };
        }

        static int[] RandArr(int count, Random rnd)
        {
            var arr = new int[count];

            for (var i = 0; i++ < count - 1; )
            {
                arr[i] = rnd.Next(0, 500);
            }
            return arr;
        }
    }
}