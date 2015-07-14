using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategySort
{
    public interface ISortingAlgorithm
    {
        void Sort(int[] arr, int left, int right);
    }


    public class QuickSort : ISortingAlgorithm
    {
        static int MyPartition(int []arr, int left, int right)
        {
            int start = left;
            int pivot = arr[start];
            left++;
            right--;

            while (true)
            {
                while (left <= right && arr[left] <= pivot)
                    left++;

                while (left <= right && arr[right] > pivot)
                    right--;

                if (left > right)
                {
                    arr[start] = arr[left - 1];
                    arr[left - 1] = pivot;

                    return left;
                }


                int temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;

            }
        }

        public void Sort(int[] arr, int left, int right)
        {
            if (arr == null || arr.Length <= 1)
                return;

            if (left < right)
            {
                int pivotIdx = MyPartition(arr, left, right);
                Sort(arr, left, pivotIdx - 1);
                Sort(arr, pivotIdx, right);
            }
        }

    }

    public class MergeSort : ISortingAlgorithm
    {
        static void MainMerge(int[] numbers, int left, int mid, int right)
        {
            int[] temp = new int[25];
            int i, eol, num, pos;

            eol = (mid - 1);
            pos = left;
            num = (right - left + 1);

            while ((left <= eol) && (mid <= right))
            {
                if (numbers[left] <= numbers[mid])
                    temp[pos++] = numbers[left++];
                else
                    temp[pos++] = numbers[mid++];
            }

            while (left <= eol)
                temp[pos++] = numbers[left++];

            while (mid <= right)
                temp[pos++] = numbers[mid++];

            for (i = 0; i < num; i++)
            {
                numbers[right] = temp[right];
                right--;
            }
        }

        public void Sort(int[] numbers, int left, int right)
        {
            int mid;

            if (right > left)
            {
                mid = (right + left) / 2;
                Sort(numbers, left, mid);
                Sort(numbers, (mid + 1), right);

                MainMerge(numbers, left, (mid + 1), right);
            }
        }
    }


    class Program
    {
        static Random rnd = new Random();

        public static int[] GetArray(int length)
        {
            var arr = new int[length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rnd.Next(-1, 10);
            }
            return arr;
        }

        public static void PrintArray(int[] arr)
        {
            foreach (var el in arr)
            {
                Console.Write("{0} ", el);
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            var arr = GetArray(10);
            PrintArray(arr);

            ISortingAlgorithm strategy = null;
            if (arr.Length <= 10)
            {
                Console.WriteLine("QuickSort");
                strategy = new QuickSort();
            }
            else
            {
                Console.WriteLine("MergeSort");
                strategy = new MergeSort();
            }

            strategy.Sort(arr, 0, arr.Length - 1);
            PrintArray(arr);

        }
    }
}
