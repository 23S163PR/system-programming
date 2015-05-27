using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sort
{
    public static class Sorts
    {
        public static int[] BubbleSort(int[] arr)
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
        public static int[] QuickSort(int[] arr, int left, int right)
        {
            if (left == right) return arr;
            int i = left + 1;
            int j = right;
            int pivot = arr[left];

            while (i < j)
            {
                if (arr[i] <= pivot) i++;
                else if (arr[j] > pivot) j--;
                else
                {
                    int m = arr[i];
                    arr[i] = arr[j];
                    arr[j] = m;
                }
            }

            if (arr[j] <= pivot)
            {
                int m = arr[left];
                arr[left] = arr[right];
                arr[right] = m;
                QuickSort(arr, left, right - 1);
            }
            else
            {
                QuickSort(arr, left, i - 1);
                QuickSort(arr, i, right);
            }

            return arr;
        }


        public static int[] SelectionSort(int[] arr)
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
        public static void mergeArray(int[] arr, int start, int mid, int end)
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
        public static int[] MergeSort(int[] arr, int start, int end)
        {
            if (start < end)
            {
                int mid = (end + start) / 2;
                MergeSort(arr, start, mid);
                MergeSort(arr, mid + 1, end);
                mergeArray(arr, start, mid, end);
            }
            return arr;
        }

    }
}
