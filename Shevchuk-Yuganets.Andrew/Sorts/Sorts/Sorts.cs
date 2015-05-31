namespace Sorts
{
	public static class Sorts
	{
		public static string BubleSort(int[] array)
		{
			for (var i = 0; i < array.Length; i++)
			{
				for (var j = 0; j < array.Length - 1; j++)
				{
					if (array[j] > array[j + 1])
					{
						var temp = array[j + 1];
						array[j + 1] = array[j];
						array[j] = temp;
					}
				}
			}
			return "Buble Sort";
		}

		public static string QuickSort(int[] array, int left, int right)
		{
			if (left == right)
				return "Qucik Sort";

			int i = left + 1;
			int j = right;
			int pivot = array[left];

			while (i < j)
			{
				if (array[i] <= pivot) i++;
				else if (array[j] > pivot) j--;
				else
				{
					int m = array[i];
					array[i] = array[j];
					array[j] = m;
				}
			}

			if (array[j] <= pivot)
			{
				int m = array[left];
				array[left] = array[right];
				array[right] = m;
				QuickSort(array, left, right - 1);
			}
			else
			{
				QuickSort(array, left, i - 1);
				QuickSort(array, i, right);
			}
			return "Qucik Sort";
		}

		public static string SelectionSort(int[] array)
		{
			for (int i = 0; i < array.Length - 1; i++)
			{
				var posMin = i;

				for (int j = i + 1; j < array.Length; j++)
				{
					if (array[j] < array[posMin])
					{
						posMin = j;
					}
				}

				if (posMin != i)
				{
					var temp = array[i];
					array[i] = array[posMin];
					array[posMin] = temp;
				}
			}

			return "Selection Sort";
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

		public static string MergeSort(int[] array, int start, int end)
		{
			if (start < end)
			{
				var mid = (end + start) / 2;
				MergeSort(array, start, mid);
				MergeSort(array, mid + 1, end);
				MergeArray(array, start, mid, end);
			}

			return "Merge Sort";
		}
	}
}
