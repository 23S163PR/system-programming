using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListProcesses.ItemsSource = Manager.GetProcess();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
            ListProcesses.Items.SortDescriptions.Add(new SortDescription());
            // Add items to ItemsSource in ListView
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListProcesses.ItemsSource);
            //view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            // Add sorting by ascending with field "Name"
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ListProcesses.Items.Refresh();
            var selectedproc = ListProcesses.SelectedItems;
            ListProcesses.ItemsSource = Manager.GetProcess();
            ListProcesses.Items.Refresh();
        }


        public class ProcessModel
        {
            public string Name { get; set; }
            public int Thread { get; set; }
            public string Memory { get; set; }
            public string Cpu { get; set; }
            public int ProcID { get; set; }

            public ProcessModel(string name, int thread, float memory, string cpu, int procID)
            {
                Name = name;
                Thread = thread;
                Cpu = cpu;
                ProcID = procID;
                //Memory = memory;
                Memory = ConvertMemory.ShorteningMemory(memory);
            }
        }
        public class ConvertMemory
        {
            public const float _1KB = 1024;
            public const float _1MB = 1048576;
            public const float _1GB = 1073741824;
            public static string ShorteningMemory(float MemoryInBytes)
            {
                float m = MemoryInBytes;
                string result = m > _1KB && m < _1MB ? (m / _1KB).ToString("F2") + " KB"
                                : m > _1MB && m < _1GB ? (m / _1MB).ToString("F2") + " MB"
                                : (m / _1GB).ToString("F2") + " GB";
                return result;
            }

        }

        public class Manager
        {
            public static List<ProcessModel> GetProcess()
            {
                List<ProcessModel> processes = new List<ProcessModel>();
                foreach (var process in Process.GetProcesses())
                {
                    if (process.Id == 0) continue;

                    if (process.ProcessName != "Idle")
                    {
                        // WorkingSet64 don't overlap with Windows Task manager
                        try
                        {
                            processes.Add(new ProcessModel(
                                process.ProcessName,
                                process.Threads.Count,
                                //process.UserProcessorTime.Milliseconds,
                                new PerformanceCounter("Process", "Working set - Private", process.ProcessName).NextValue(),
                                new PerformanceCounter("Process", "% Processor Time", process.ProcessName).NextValue().ToString("F2"),
                                process.Id));
                        }
                        catch (Exception e)
                        {}
                    }
                }
                return processes;
            } 
        }

        private void CloseProcess_Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListProcesses.SelectedItem is ProcessModel)
            {
                var a = Process.GetProcessById((ListProcesses.SelectedItem as ProcessModel).ProcID);
                a.Kill();
            }
        }

        public void Refresh(object o, RoutedEventArgs e)
        {
            var selectedproc = ListProcesses.SelectedItems;
            ListProcesses.ItemsSource = Manager.GetProcess();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListProcesses.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
    }
}
