using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SortAlgorithm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<int> _sizesArray = new List<int>
            {
                10,
                100,
                1000,
                10000
            };
        private int _sizeArray;
        private int[] _arrBubbleSort;
        private int[] _arrMergeSort;
        private int[] _arrQuickSort;
        private int[] _arrSelectionSort;
        private readonly Random _rand = new Random();
        private List<Task<long>> _tasks = new List<Task<long>>();

        public MainWindow()
        {
            InitializeComponent();
            InitialComboBox();
        }
       
        public void InitialComboBox()
        {
            foreach (var size in _sizesArray)
            {
                SizeArray.Items.Add(size);
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartSorting.IsEnabled = false;
            GenerateArray.IsEnabled = SizeArray.SelectedIndex >= 0;
            _sizeArray = (int)SizeArray.SelectedValue;
        }

        #region Generate Arrays
        private void GenerateArray_OnClick(object sender, RoutedEventArgs e)
        {
            StartSorting.IsEnabled = false;
            GenerateArray.IsEnabled = false;
            var generate = new Task(Generate);
            generate.Start();
            generate.ContinueWith((t) => Dispatcher.BeginInvoke(new ThreadStart(UnLockSortAndGenerate)));
        }

        private void Generate()
        {
            _arrBubbleSort = new int[_sizeArray];
            _arrMergeSort = new int[_sizeArray];
            _arrQuickSort = new int[_sizeArray];
            _arrSelectionSort = new int[_sizeArray];
            for (var i = 0; i < _sizeArray; i++)
            {
                _arrBubbleSort[i] = _rand.Next(-_sizeArray, _sizeArray);
            }
            Array.Copy(_arrBubbleSort, _arrMergeSort, _sizeArray - 1);
            Array.Copy(_arrBubbleSort, _arrQuickSort, _sizeArray - 1);
            Array.Copy(_arrBubbleSort, _arrSelectionSort, _sizeArray - 1);
        }

        private void UnLockSortAndGenerate()
        {
            GenerateArray.IsEnabled = true;
            StartSorting.IsEnabled = _arrBubbleSort.Length > 0;
        }
        #endregion
        

        #region Sorting and Visualization
        private void StartSorting_OnClick(object sender, RoutedEventArgs e)
        {
            StartSorting.IsEnabled = false;
            GenerateArray.IsEnabled = false;
            SizeArray.IsEnabled = false;
            Task sort = new Task(Sort);
            sort.Start();
            sort.ContinueWith((t) => Dispatcher.BeginInvoke(new ThreadStart(HiddenProgress)));
            sort.ContinueWith((t) => Dispatcher.BeginInvoke(new ThreadStart(CreateGraphic)));
            
        }
        async void Sort()
        {
            _tasks = new List<Task<long>>
            {
               GetLeadTime(() => Sortings.BubbleSort(_arrBubbleSort)),
               GetLeadTime(() => Sortings.MergeSort(_arrMergeSort, 0, _sizeArray - 1)),
               GetLeadTime(() => Sortings.QuickSort(_arrQuickSort, 0, _sizeArray - 1)),
               GetLeadTime(() => Sortings.SelectionSort(_arrSelectionSort)),
            };
            for (int i = 0; i < _tasks.Count; i++)
            {
                await Task.WhenAny(_tasks[i]);
            }
        }

        async Task<long> GetLeadTime(Func<int[]> sorting)
        {
            var stopWotch = new Stopwatch();
            stopWotch.Start();
            sorting();
            stopWotch.Stop();
            return stopWotch.ElapsedTicks;
        }

        private void HiddenProgress()
        {
            StartSorting.IsEnabled = true;
            GenerateArray.IsEnabled = true;
            SizeArray.IsEnabled = true;
        }

        private void CreateGraphic()
        {
            List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();
            MyValue.Add(new KeyValuePair<string, int>("Bubble Sort", (int)_tasks[0].Result));
            MyValue.Add(new KeyValuePair<string, int>("Merge Sort", (int)_tasks[1].Result));
            MyValue.Add(new KeyValuePair<string, int>("Quick Sort", (int)_tasks[2].Result));
            MyValue.Add(new KeyValuePair<string, int>("Selection Sort", (int)_tasks[3].Result));
            PieChart1.DataContext = MyValue;
        }

       
#endregion
    }
    
}
