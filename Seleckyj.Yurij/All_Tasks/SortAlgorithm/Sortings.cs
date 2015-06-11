using System.Threading;

namespace SortAlgorithm
{
    public static class Sortings // copy paste from www
    {
        public static int[] BubbleSort(int[] arr)
        {
            for (int j = 0; j < arr.Length - 1; j++)
            {
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        int b = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = b;
                    }
                }
            }
            return arr;
        }

        public static int[] SelectionSort(int[] arr)
        {
            for (var i = 0; i < arr.Length - 1; i++)
            {
                var min = i;
                for (var j = i + 1; j < arr.Length; j++)
                {
                    if (arr[j] < arr[min])
                    {
                        min = j;
                    }
                }
                var temp = arr[i];
                arr[i] = arr[min];
                arr[min] = temp;
            }
            return arr;
        }

        public static int[] QuickSort(int[] arr, int left, int right)
        {
            if (left == right) return arr;
            var i = left + 1;
            var j = right;
            var pivot = arr[left];

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

        private static void MergeArray(int[] arr, int start, int mid, int end)
        {
            int[] temp = new int[end - start + 1];

            int i = start, j = mid + 1, k = 0;
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
            while (i <= mid)
            {
                temp[k] = arr[i];
                k++;
                i++;
            }
            while (j <= end)
            {
                temp[k] = arr[j];
                k++;
                j++;
            }
            k = 0;
            i = start;
            while (k < temp.Length && i <= end)
            {
                arr[i] = temp[k];
                i++;
                k++;
            }
        }

        public static int[] MergeSort(int[] arr, int start, int end)
        {
            if (start < end)
            {
                int mid = (end + start) / 2;
                MergeSort(arr, start, mid);
                MergeSort(arr, mid + 1, end);
                MergeArray(arr, start, mid, end);
            }
            return arr;
        }
    }
}
