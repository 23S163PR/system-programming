using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Classes;

namespace TaskManager
{

    public partial class MainWindow : Window
    {
        private ObservableCollection<ProcessInfo> _processesCollection;
        public ObservableCollection<ProcessInfo> ProcessesCollection { get { return _processesCollection; } }
        System.Windows.Threading.DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _processesCollection = new ObservableCollection<ProcessInfo>();
            _timer = new System.Windows.Threading.DispatcherTimer();

            this.DataContext = this;

            InitData();
            

        }

        private void InitData()
        {
            
            foreach (var el in Process.GetProcesses())
            {
                if (!PerformanceCounterCategory.Exists(el.ProcessName))
                {
                    _processesCollection.Add(new ProcessInfo(el.Id
                                                    , el.ProcessName
                                                    , el.Threads.Count
                                                    , new PerformanceCounter("Process", "Working Set - Private", el.ProcessName).RawValue
                                                    , new PerformanceCounter("Process", "% Processor Time", el.ProcessName)));
                }
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            _timer.Tick += EventTick;
            _timer.Interval = new TimeSpan(0, 0, 0, 1);
            _timer.Start();
        }

        private void EventTick(object sender, EventArgs e)
        {
            _timer.Stop();
            UpdateInfo();
            _timer.Start();
        }


        private void UpdateInfo()
        {
            DataGridColumn columnSorted = dGrid.Columns.FirstOrDefault(x => x.SortDirection != null);
            SortDescription sDescription = new SortDescription();
            if (columnSorted != null)
            {
                sDescription = new SortDescription(columnSorted.SortMemberPath, columnSorted.SortDirection.Value);
            }
            //////

            // Remove processes
            var processes = Process.GetProcesses().ToList();
            for (int i = ProcessesCollection.Count - 1; i >= 0; i--)
            {
                var proc = processes.FirstOrDefault(x => x.Id == ProcessesCollection[i].ProcessId);
                if (proc == null)
                {
                    ProcessesCollection.RemoveAt(i);
                }
            }

            // Add and Update processes
            foreach (var el in processes)
            {
                ProcessInfo proc = ProcessesCollection.FirstOrDefault(x => x.ProcessId == el.Id);
                if (proc == null)
                {
                        ProcessesCollection.Add(new ProcessInfo(el.Id
                                                    , el.ProcessName
                                                    , el.Threads.Count
                                                    , new PerformanceCounter("Process", "Working Set - Private", el.ProcessName).RawValue
                                                    , new PerformanceCounter("Process", "% Processor Time", el.ProcessName)));

                }
                else
                {
                    //var workingSet = new PerformanceCounter("Process", "Working Set - Private", el.ProcessName).RawValue;
                    proc.UpdateData(el.Threads.Count
                                    , new PerformanceCounter("Process", "Working Set - Private", el.ProcessName).RawValue);
                }
            }



            if (sDescription.PropertyName != null)
            {
                CollectionViewSource.GetDefaultView(dGrid.Items).SortDescriptions.Add(sDescription);
                columnSorted.SortDirection = sDescription.Direction;
            }
        }
    }
}
