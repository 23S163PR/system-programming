using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sort
{
    class Program
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
				taskFactory.StartNew(() => GetRuntime(() => bubbleSort(bubble))),
				taskFactory.StartNew(() => GetRuntime(() => Quicksort(quick,0,quick.Length-1))),
				taskFactory.StartNew(() => GetRuntime(() => SelectionSort(selection))),
				taskFactory.StartNew(() => GetRuntime(() => mergesort(merge,0,merge.Length-1)))
			};

			foreach (var time in tasks)
			{
				Console.WriteLine("{0, -10} Miliseconds \n", time.Result);
			}

        }

        static int[] bubbleSort(int[] arr)
        {
            for (int j = 0; j < arr.Length - 1; j++)
            {
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        int b = arr[i]; //change for elements
                        arr[i] = arr[i + 1];
                        arr[i + 1] = b;
                    }
                }
            }
            return arr;
        }
        static double GetRuntime(Func<int[]> sortMethod)
		{
			var stopWotch = new Stopwatch();
			stopWotch.Start();
			sortMethod();
			stopWotch.Stop();
			return stopWotch.Elapsed.TotalMilliseconds;
		}

        static int[] Quicksort(int[] arr, int left, int right)
        {
            int i = left, j = right;
            IComparable pivot = arr[(left + right) / 2];

            while (i <= j)
            {
                while (arr[i].CompareTo(pivot) < 0)
                {
                    i++;
                }

                while (arr[j].CompareTo(pivot) > 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    // Swap
                    int tmp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = tmp;

                    i++;
                    j--;
                }
            }

            // Recursive calls
            if (left < j)
            {
                Quicksort(arr, left, j);
            }

            if (i < right)
            {
                Quicksort(arr, i, right);
            }
            return arr;
        }

        static int[] SelectionSort(int[] arr)
        {
            int i, j, min, temp;
            for (i = 0; i < arr.Length - 1; i++)
            {
                min = i;
                for (j = i + 1; j < arr.Length; j++)
                {
                    if (arr[j] < arr[min])
                    {
                        min = j;
                    }
                }
                temp = arr[i];
                arr[i] = arr[min];
                arr[min] = temp;
            }
            return arr;
        }
        static void mergeArray(int[] arr, int start, int mid, int end)
        {
            /* Create a temporary array for stroing merged array (Length of temp array will be 
             * sum of size of both array to be merged)*/
            int[] temp = new int[end - start + 1];

            int i = start, j = mid + 1, k = 0;
            // Now traverse both array simultaniously and store the smallest element of both to temp array
            while (i <= mid && j <= end)
            {
                if (arr[i] < arr[j])
                {
                    temp[k] = arr[i];
                    k++;
                    i++;
                }
                else
                {
                    temp[k] = arr[j];
                    k++;
                    j++;
                }
            }
            // If there is any element remain in first array then add it to temp array
            while (i <= mid)
            {
                temp[k] = arr[i];
                k++;
                i++;
            }
            // If any element remain in second array then add it to temp array
            while (j <= end)
            {
                temp[k] = arr[j];
                k++;
                j++;
            }
            // Now temp has merged sorted element of both array

            // Traverse temp array and store element of temp array to original array
            k = 0;
            i = start;
            while (k < temp.Length && i <= end)
            {
                arr[i] = temp[k];
                i++;
                k++;
            }
        }
        // Recursive Merge Procedure
        static int[] mergesort(int[] arr, int start, int end)
        {
            if (start < end)
            {
                int mid = (end + start) / 2;
                mergesort(arr, start, mid);
                mergesort(arr, mid + 1, end);
                mergeArray(arr, start, mid, end);
            }
            return arr;
        }

    }
}


