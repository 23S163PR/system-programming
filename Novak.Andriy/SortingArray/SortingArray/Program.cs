using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SortingArray
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var bubleArr = RandArr(5000, new Random());
            var quickArr = (int[])bubleArr.Clone();
            var selectionArr = (int[])bubleArr.Clone();
            var mergArr = (int[])bubleArr.Clone();
            var task = new TaskFactory(
                TaskCreationOptions.AttachedToParent,
                TaskContinuationOptions.None);
            //inclusive 40%
            var tasks = new[]
            {
                /*inclusive samples 20%*/
                 task.StartNew(() => GetTime(() => SortArray.BubleSort(bubleArr)))
                 /*inclusive samples 10%*/  // 
                 ,task.StartNew(() => GetTime(() => SortArray.QuickSort(quickArr,0,quickArr.Length-1)))
                //samples 14.29
                ,task.StartNew(() => GetTime(() => SortArray.SelectionSort(selectionArr)))
                /*inclusive samples 30%*/
                ,task.StartNew(() => GetTime(() => SortArray.MergeSort(mergArr,0,mergArr.Length-1)))   
            };
           
            foreach (var t in tasks)
            {
                Console.WriteLine("\n{0} Miliseconds", t.Result);
            }
        }

        static double GetTime(Func<int[]> func)
        {
            var stopWotch = new Stopwatch();
            stopWotch.Start();
            func();
            stopWotch.Stop();
            return stopWotch.Elapsed.TotalMilliseconds;
        }

        static int[] RandArr(int count, Random rnd)
        {
            var arr = new int[count];
          
            for (var i = 0; i++ < count-1;)
            {
                arr[i] = rnd.Next(0, 500);
            }
            return arr;
        }
    }
}
