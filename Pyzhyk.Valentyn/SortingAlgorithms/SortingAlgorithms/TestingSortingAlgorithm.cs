using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    public class TestingSortingAlgorithm
    {
        public static int[] GetArray(int count, int maxValue)
        {
            Random random = new Random();
            var array = new int[count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(maxValue);
            }
            return array;
        }
        public static int[] CloneArray(int[] array)
        {
            var newarray = new int[array.Length];
            for (int i = 0; i < newarray.Length; i++)
            {
                newarray[i] = array[i];
            }
            return newarray;
        }
        public static void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(array[i]);
            }
            Console.WriteLine();
        }
        public static double GetTime(Func<int[]> function)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            function();
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        private static void Swap(int valueOne, int valueTwo)
        {
            valueOne = valueOne + valueTwo;
            valueTwo = valueOne - valueTwo;
            valueOne = valueOne - valueTwo;
        }
        // Buble Sorting
        public static int[] BubbleSort(int[] inputArray)
        {
            for (int iterator = 0; iterator < inputArray.Length; iterator++)
            {
                for (int index = 0; index < inputArray.Length - 1; index++)
                {
                    if (inputArray[index] > inputArray[index + 1])
                    {
                        Swap(inputArray[index], inputArray[index + 1]);
                    }
                }
            }
            return inputArray;
        }
        // Selection Sorting
        public static int[] SelectionSort(int[] inputArray)
        {
            long index_of_min = 0;
            for (int iterator = 0; iterator < inputArray.Length - 1; iterator++)
            {
                index_of_min = iterator;
                for (int index = iterator + 1; index < inputArray.Length; index++)
                {
                    if (inputArray[index] < inputArray[index_of_min])
                        index_of_min = index;
                }
                Swap(inputArray[iterator], inputArray[index_of_min]);
            }
            return inputArray;
        }
        // Merge Sorting
        public static int[] MergeSort(int[] inputArray, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;

                MergeSort(inputArray, left, middle);
                MergeSort(inputArray, middle + 1, right);

                //Merge
                int[] leftArray = new int[middle - left + 1];
                int[] rightArray = new int[right - middle];

                Array.Copy(inputArray, left, leftArray, 0, middle - left + 1);
                Array.Copy(inputArray, middle + 1, rightArray, 0, right - middle);

                int i = 0;
                int j = 0;
                for (int k = left; k < right + 1; k++)
                {
                    if (i == leftArray.Length)
                    {
                        inputArray[k] = rightArray[j];
                        j++;
                    }
                    else if (j == rightArray.Length)
                    {
                        inputArray[k] = leftArray[i];
                        i++;
                    }
                    else if (leftArray[i] <= rightArray[j])
                    {
                        inputArray[k] = leftArray[i];
                        i++;
                    }
                    else
                    {
                        inputArray[k] = rightArray[j];
                        j++;
                    }
                }
            }
            return inputArray;
        }
        // Quick Sorting
        public static int[] QuickSort(int[] elements, int left, int right)
        {
            int i = left, j = right;
            int pivot = elements[(left + right) / 2];

            while (i <= j)
            {
                while (elements[i].CompareTo(pivot) < 0)
                {
                    i++;
                }

                while (elements[j].CompareTo(pivot) > 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    // Swap
                    var tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    i++;
                    j--;
                }
            }
            // Recursive calls
            if (left < j)
            {
                QuickSort(elements, left, j);
            }

            if (i < right)
            {
                QuickSort(elements, i, right);
            }

            return elements;
        }
    }


}
