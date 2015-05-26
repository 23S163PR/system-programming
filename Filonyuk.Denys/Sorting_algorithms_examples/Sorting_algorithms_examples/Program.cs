using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithms_examples
{
    class Program
    {
        static void Main(string[] args)
        {
            const int size = 10000;
            int[] unsorted_array = new int[size];
            Random Rnd_digit = new Random();

            for (int i = 0; i < unsorted_array.Length; i++)
            {
                unsorted_array[i] = Rnd_digit.Next(0, 1000);
            }
            Stopwatch obj1 = new Stopwatch();



            
            Task QuickSortingTask = new Task(() =>
                {
                    int[] CopyOfBaseArr = new int[size];
                    unsorted_array.CopyTo(CopyOfBaseArr,0);

                    obj1.Start();
                    Quicksort(CopyOfBaseArr,0,CopyOfBaseArr.Length-1);
                    obj1.Stop();
                    Console.WriteLine(" Quick sorting of {0} elements \n ElapsedMilliseconds {1}\n",CopyOfBaseArr.Length, obj1.ElapsedMilliseconds);
                });
            
            QuickSortingTask.Start();
            QuickSortingTask.Wait();

            Task BubleSortingTask = new Task(() =>
            {
                int[] CopyOfBaseArr = new int[size];
                unsorted_array.CopyTo(CopyOfBaseArr, 0);

                obj1.Start();
                BubleSort(CopyOfBaseArr);
                obj1.Stop();
                Console.WriteLine(" Bubble sorting of {0} elements \n ElapsedMilliseconds {1}\n", CopyOfBaseArr.Length, obj1.ElapsedMilliseconds);
            });

            BubleSortingTask.Start();
            BubleSortingTask.Wait();

            Task SelectSortingTask = new Task(() =>
            {
                int[] CopyOfBaseArr = new int[size];
                unsorted_array.CopyTo(CopyOfBaseArr, 0);

                obj1.Start();
                selectsort(CopyOfBaseArr, CopyOfBaseArr.Length);
                obj1.Stop();
                Console.WriteLine(" Select sorting of {0} elements \n ElapsedMilliseconds {1}\n", CopyOfBaseArr.Length, obj1.ElapsedMilliseconds);
            });

            SelectSortingTask.Start();
            SelectSortingTask.Wait();
            //Console.WriteLine("ElapsedMilliseconds {0}", obj1.ElapsedMilliseconds);

           // Console.WriteLine("Before sorting");

           // //foreach (var el in unsorted_array)
           // //{
           // //    Console.WriteLine(el.ToString());
           // //}
           //obj1.Start();//30820
           //Console.WriteLine("After Bubble sorting");
           //BubleSort(unsorted_array);
           
           //obj1.Stop();
           //Console.WriteLine("ElapsedMilliseconds {0}",obj1.ElapsedMilliseconds);


          // Console.WriteLine("After Quick sort");
          // obj1.Start();//71
          // Quicksort(unsorted_array, 0, unsorted_array.Length - 1);
          // 
          // obj1.Stop();
          // Console.WriteLine("ElapsedMilliseconds {0}", obj1.ElapsedMilliseconds);


          // Console.WriteLine("After select sort");
          // obj1.Start();//11120
          // selectsort(unsorted_array, unsorted_array.Length);
          // obj1.Stop();
          // Console.WriteLine("ElapsedMilliseconds {0}", obj1.ElapsedMilliseconds);


            //Console.WriteLine("After merge_recursive sort");  
            //obj1.Start();
            //MergeSort_Recursive(unsorted_array, 0, unsorted_array.Length-1 );
            //obj1.Stop();
            //Console.WriteLine("ElapsedMilliseconds {0}", obj1.ElapsedMilliseconds);

        }

        public static void BubleSort(int[] arr)
        {
            int temp = 0;

            for (int write = 0; write < arr.Length; write++)
            {
                for (int sort = 0; sort < arr.Length - 1; sort++)
                {
                    if (arr[sort] > arr[sort + 1])
                    {
                        temp = arr[sort + 1];
                        arr[sort + 1] = arr[sort];
                        arr[sort] = temp;
                    }
                }
            }
           // return arr;
        }

        public static void Quicksort(int[] elements, int left, int right)
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
                    int tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    i++;
                    j--;
                }
            }

            // Recursive calls
            if (left < j)
            {
                Quicksort(elements, left, j);
            }

            if (i < right)
            {
                Quicksort(elements, i, right);
            }
        }



        static void selectsort(int[] dataset, int n){
                int i,j;
                for(i=0;i<n;i++){
                       int min=i;     
                       for(j=i+1;j<n;j++)
                         if(dataset[j]<dataset[min]) min=j; //find min value
                          //then swap it with the beginning item of the unsorted list
                           int temp=dataset[i];
                          dataset[i]=dataset[min];
                          dataset[min]=temp;
                 }

        }


        static public void DoMerge(int[] numbers, int left, int mid, int right)
        {
            int[] temp = new int[25];
            int i, left_end, num_elements, tmp_pos;

            left_end = (mid - 1);
            tmp_pos = left;
            num_elements = (right - left + 1);

            while ((left <= left_end) && (mid <= right))
            {
                if (numbers[left] <= numbers[mid])
                    temp[tmp_pos++] = numbers[left++];
                else
                    temp[tmp_pos++] = numbers[mid++];
            }

            while (left <= left_end)
                temp[tmp_pos++] = numbers[left++];

            while (mid <= right)
                temp[tmp_pos++] = numbers[mid++];

            for (i = 0; i < num_elements; i++)
            {
                numbers[right] = temp[right];
                right--;
            }
        }

        static public void MergeSort_Recursive(int[] numbers, int left, int right)
        {
            int mid;

            if (right > left)
            {
                mid = (right + left) / 2;
                MergeSort_Recursive(numbers, left, mid);
                MergeSort_Recursive(numbers, (mid + 1), right);

                DoMerge(numbers, left, (mid + 1), right);
            }
        }
   }
 }
    


