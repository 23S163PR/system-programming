using System.Diagnostics;
using System.Windows;
using TaskManager.Classes;
using System.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for TaskManagerWindow.xaml
    /// </summary>
    public partial class TaskManagerWindow : Window
    {
        private ObservableCollection<ProcessInfo> _processes;

        public ObservableCollection<ProcessInfo> Processes
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

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += (s, e) => UpdateProcessesInfo();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }

        private ObservableCollection<ProcessInfo> GetProcesses()
        {
            var collection = new ObservableCollection<ProcessInfo>();
            foreach (var el in Process.GetProcesses())
            {
                var workingSetCounter = GetPerformanceCounter(el, "Working Set - Private");
                if (workingSetCounter == null) continue;
                var cpuUssingCounter = GetPerformanceCounter(el, "% Processor Time");
                collection.Add(new ProcessInfo(el.Id
                                         , el.ProcessName
                                         , el.Threads.Count
                                         , workingSetCounter.RawValue
                                         , cpuUssingCounter));

            }
            return collection;
        }

        private void UpdateProcessesInfo()
        {
            foreach (var el in Process.GetProcesses())
            {
                ProcessInfo proc = Processes.FirstOrDefault(x => x.ProcessId == el.Id);
                var workingSetCounter = GetPerformanceCounter(el, "Working Set - Private");
                if (workingSetCounter == null) continue;
                if (proc == null)
                {
                    var cpuUssingCounter = GetPerformanceCounter(el, "% Processor Time");
                    Processes.Add(new ProcessInfo(el.Id
                                                  , el.ProcessName
                                                  , el.Threads.Count
                                                  , workingSetCounter.RawValue
                                                  , cpuUssingCounter));
                }
                else
                {
                    proc.UpdateInfo(el.Threads.Count
                                    , workingSetCounter.RawValue);
                }

            }
            for (int i = 0; i < Processes.Count; i++)
            {
                var proc = Process.GetProcesses().FirstOrDefault(x => x.Id == Processes[i].ProcessId);
                if (proc == null)
                {
                    Processes.Remove(Processes[i]);
                }
            }
        }

        public PerformanceCounter GetPerformanceCounter(Process process, string processCounterName)
        {
            var processName = Path.GetFileNameWithoutExtension(process.ProcessName);
            var processCategory = new PerformanceCounterCategory("Process");
            var similarInstances = processCategory.GetInstanceNames()
                .Where(instance => instance.StartsWith(processName));

            foreach (var instance in similarInstances)
            {
                using (var processIdCounter = new PerformanceCounter("Process", "ID Process", instance, true /* readOnly */))
                {
                    int instanceId = (int)processIdCounter.RawValue;
                    if (instanceId == process.Id)
                    {
                        return new PerformanceCounter("Process", processCounterName, instance);
                    }
                }
            }
            return null;
        }
    }
}
