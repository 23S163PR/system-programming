using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace taskmsg
{
	class TaskManager
	{
        private readonly ObservableCollection<ProcessModel> _procs;

		public int CurentProcessId { get; set; }

		public TaskManager()
		{
            _procs = new ObservableCollection<ProcessModel>();
			RefreshProcesses();
		}

        public ObservableCollection<ProcessModel> Processes
		{
			get
			{
				return _procs;
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

	    private void ClearOld(IEnumerable<Process> processes)
	    {
            var id = processes.Select(t => t.Id);
	        var dels =  _procs.Where(p => !id.Contains(p.Id)).ToList();
	        if (!dels.Any()) return;
	        foreach (var item in dels)
	        {
	            _procs.Remove(item);
	        }
	    }

        public void RefreshProcesses()
        {
            var proc = Process.GetProcesses();
            var res = proc.Where(t => t.ProcessName != "Idle").OrderBy(p => p.ProcessName).ThenBy(p => p.Id);
            ClearOld(res);
            //var task = new Task<ICollection<KeyValuePair<int, string>>>(GetProcessList, TaskCreationOptions.DenyChildAttach);
            //task.Start();
            foreach (var p in res)
            {
                var curent = _procs.FirstOrDefault(c => c.Id == p.Id);
               // var persentLoad = task.Result.First(t => t.Key == p.Id).Value;
                if (curent != null)
                {
                    curent = ProcessModel.CompareChanger(curent, p, ""/*persentLoad*/);
                }
                else
                {
                    var process = new ProcessModel
                        (
                        p.Id
                        , p.ProcessName
                        , (p.WorkingSet64 / 1024f) / 1024f
                        , p.Threads.Count
                        , GetProcessTime(p.Id)
                        ,""//persentLoad
                        );
                    _procs.Add(process);
                }
            }
        }

        private static ICollection<KeyValuePair<int,string>> GetProcessList()
		{
            var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfProc_Process");
			var collection = new ConcurrentDictionary<int,string>();

            Parallel.ForEach(searcher.Get().Cast<ManagementObject>()
                .Where(obj => obj["Name"].ToString() != "Idle")
			,(obj =>
            {
                 collection.TryAdd(int.Parse(obj["IDProcess"].ToString()),
			        string.Format("{0}%", obj["PercentProcessorTime"].ToString()));
            })); 
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
		    try
		    {
		        GetProcess(id).PriorityClass = priority;
		    }
		    catch (Exception)
		    {
		        //if not acsees to system process
		    }
		}

		public void CloseProcess(int id)
		{
			try
			{
				GetProcess(id).Kill();
				var item = _procs.FirstOrDefault(p => p.Id == id);
				_procs.Remove(item);
			}
			catch(Exception)
            {
                //if not acsees to system process
            }
		}
	}
}
