using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ProcessManager
{
	class TaskManager
	{
        private readonly ObservableCollection<ProcessModel> _processes;
	    private static readonly Dispatcher _dispatcher = Application.Current.Dispatcher;
		public int CurentProcessId { get; set; }

		public TaskManager()
		{
            _processes = new ObservableCollection<ProcessModel>();
			RefreshProcesses();
		}

        public ObservableCollection<ProcessModel> Processes
		{
			get
			{
				return _processes;
			}
		}

		public static string GetProcessTime(int id)
		{
			var time = new TimeSpan();
			try
			{
				time = Process.GetProcessById(id).TotalProcessorTime;
			}
			catch (Exception)
			{
				//if we do not have access to the system process
                return time.ToString(@"hh\:mm\:ss"); 
			}
            return time.ToString(@"hh\:mm\:ss"); 
		}

	    private void ClearOldProcesses(IEnumerable<ProcessModel> processes)
	    {
            var id = processes.Select(t => t.Id);
	        var deletes =  _processes.Where(p => !id.Contains(p.Id)).ToList();
	        if (!deletes.Any()) return;
	        
	        foreach (var item in deletes)
	        {
	           _dispatcher.Invoke(() => _processes.Remove(item));
	        }
	    }

        public void RefreshProcesses()
        {
            var getProcessTask = new Task<ObservableCollection<ProcessModel>>(GetProcessList);
            getProcessTask.Start();

            var processList =  getProcessTask.Result;
            ClearOldProcesses(processList);
            foreach (var process in processList)
            {
                var curent = _processes.FirstOrDefault(c => c.Id == process.Id);
            
                if (curent != null)
                {
                    curent = ProcessModel.CompareChanger(curent, process);
                }
                else
                {
                    _dispatcher.Invoke(() => _processes.Add(process));
                }
            }
        }

        private static ObservableCollection<ProcessModel> GetProcessList()
		{
			var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfProc_Process");

			var collection = new ObservableCollection<ProcessModel>();

			foreach (var obj in searcher.Get()
				.Cast<ManagementObject>()
				.Where(obj => obj["Name"].ToString() != "Idle")
                .Where(obj => obj["Name"].ToString() != "_Total").OrderBy(obj => obj["Name"])
                .ThenBy(obj => obj["IDProcess"]))
			{
                var id = int.Parse(obj["IDProcess"].ToString());
			    var process = Process.GetProcessById(id);
			    var c=process.Threads.Count;
			    collection.Add(new ProcessModel
			        (   id
			            , obj["Name"].ToString()
			            , process.WorkingSet64 
			            , process.Threads.Count
			            , GetProcessTime(id)
			            , string.Format("{0}%", obj["PercentProcessorTime"])
			        ));
			}
			return collection;
		}
		
		private static Process GetProcess(int id)
		{
			return Process.GetProcessById(id);
		}

		public ProcessPriorityClass GetProcesPriorityClass(int id)
		{
			return GetProcess(id).PriorityClass;
		}

        public static void SetProcessPrioruty(int id, ProcessPriorityClass priority)
		{	    
		    GetProcess(id).PriorityClass = priority;
		}

		public void CloseProcess(int id)
		{
			GetProcess(id).Kill();
			var item = _processes.FirstOrDefault(p => p.Id == id);
			_processes.Remove(item);
         }
	}
}
