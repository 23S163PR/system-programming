using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSpeedAlgorithmsOfSort
{
    class Program
    {
        static void InitData(ref int[] arr)
        {
            Random rnd = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rnd.Next(0, 10);
            }
        }

        static int[] BubleSort(int[] source)
        {
            var arr = source;
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        var temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
            return arr;
        }

        static int[] QuickSort(int[] source)
        {
            //var arr = source;   
            //return arr;
            return null;
        }


        static void Main(string[] args)
        {
            int[] arr = new int[1000];
            InitData(ref arr);

            //foreach (var el in arr)
            //{
            //    Console.Write(el + "  ");
            //}
        }
    }
}
