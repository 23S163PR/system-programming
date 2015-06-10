using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using TaskManager.Classes;
using System.Linq;
using System;
using System.Threading;
using System.Collections.ObjectModel;
using System.Timers;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TaskManagerWindow : Window
    {
        private System.Timers.Timer _timer;
        private List<ProcessInfo> _processes;

        public List<ProcessInfo> Processes
        {
            get { return _processes; }
            private set
            {
                if (_processes == value) return;
                _processes = value;
            }
        }


        public TaskManagerWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Processes = GetProcesses();

            //System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //dispatcherTimer.Tick += (s, e) => UpdateProcessesInfo();
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //dispatcherTimer.Start();

            //new Thread(UpdateProcessesInfo).Start();
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += (s, e) => UpdateProcessesInfo();
            _timer.Enabled = true;
            _timer.Start();
        }

        private List<ProcessInfo> GetProcesses()
        {
            var collection = new List<ProcessInfo>();
            foreach (var el in Process.GetProcesses())
            {
                try
                {
                    var workingSetInB = new PerformanceCounter("Process", "Working Set - Private", el.ProcessName).RawValue;
                    var cpuUssingCounter = new PerformanceCounter("Process", "% Processor Time", el.ProcessName);
                    collection.Add(new ProcessInfo(el.Id
                                             , el.ProcessName
                                             , el.Threads.Count
                                             , workingSetInB
                                             , cpuUssingCounter));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            return collection;
        }

        private void UpdateProcessesInfo()
        {
            foreach (var el in Process.GetProcesses())
            {
                ProcessInfo proc = Processes.FirstOrDefault(x => x.ProcessId == el.Id);
                try
                {
                    var workingSetInB = new PerformanceCounter("Process", "Working Set - Private", el.ProcessName).RawValue;
                    if (proc == null)
                    {
                        var cpuUssingCounter = new PerformanceCounter("Process", "% Processor Time", el.ProcessName);
                        Processes.Add(new ProcessInfo(el.Id
                                                      , el.ProcessName
                                                      , el.Threads.Count
                                                      , workingSetInB
                                                      , cpuUssingCounter));
                    }
                    else
                    {
                        proc.UpdateInfo(el.Threads.Count
                                        , workingSetInB);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
            for (int i = Processes.Count - 1; i >= 0; i--)
            {
                var proc = Process.GetProcesses().FirstOrDefault(x => x.Id == Processes[i].ProcessId);
                if (proc == null)
                {
                    Processes.Remove(Processes[i]);
                }
            }
        }
    }
}
