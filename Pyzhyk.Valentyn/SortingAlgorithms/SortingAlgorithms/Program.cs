using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            var array1 = TestingSortingAlgorithm.GetArray(1000000, 10);
            var array2 = TestingSortingAlgorithm.CloneArray(array1);
            var array3 = TestingSortingAlgorithm.CloneArray(array1);
            var array4 = TestingSortingAlgorithm.CloneArray(array1);
            var tf = new TaskFactory(
                TaskCreationOptions.AttachedToParent,
                TaskContinuationOptions.ExecuteSynchronously);

            var tasks = new[]
            {
                 tf.StartNew(() => TestingSortingAlgorithm.GetTime(() => TestingSortingAlgorithm.BubbleSort(array1)))
                 .ContinueWith((task) =>
                 {
                     Console.WriteLine("Bubble sort in array length: " + array1.Length + " in time: " + task.Result + "ms");
                 }),

                tf.StartNew(() => TestingSortingAlgorithm.GetTime(() => TestingSortingAlgorithm.MergeSort(array2, 0, array2.Length -1)))
                .ContinueWith((task) =>
                {
                    Console.WriteLine("Merge sort in array length: " + array2.Length + " in time: " + task.Result + "ms");
                }),
                tf.StartNew(() => TestingSortingAlgorithm.GetTime(() => TestingSortingAlgorithm.SelectionSort(array3)))
                .ContinueWith((task) =>
                {
                    Console.WriteLine("Selection sort in array length: " + array3.Length + " in time: " + task.Result + "ms");
                }),
                tf.StartNew(() => TestingSortingAlgorithm.GetTime(() => TestingSortingAlgorithm.QuickSort(array4, 0, array4.Length -1)))
                .ContinueWith((task) =>
                {
                    Console.WriteLine("Quick sort in array length: " + array4.Length + " in time: " + task.Result + "ms");
                })
            };
            //foreach (var task in tasks)
            //{
            //    Console.WriteLine(task.Result + "ms");
            //}
            Console.ReadLine();
        }
    }
}
