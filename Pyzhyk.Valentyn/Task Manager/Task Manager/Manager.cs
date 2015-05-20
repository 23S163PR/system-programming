using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Task_Manager
{
    public class Manager
    {
        public void Refresh()
        {

        }
        public static ObservableCollection<ProcessModel> GetProcess()
        {
            ObservableCollection<ProcessModel> processes = new ObservableCollection<ProcessModel>();

            foreach (var process in Process.GetProcesses())
            {
                if (process.Id == 0) continue;
                try
                {
                    processes.Add(new ProcessModel(process));
                }
                catch (Exception e)
                { }
            }
            return processes;
        }
        public static void UpdateProcessesInfo(ObservableCollection<ProcessModel> collection)
        {
            foreach (var item in collection)
            {
                try
                {
                    var newProcess = Process.GetProcessById(item.ProcessId);
                    item.SetInfoProcess(newProcess);
                }
                catch (Exception)
                {

                }
            }
        }
        public static void AddNewProcesses(ObservableCollection<ProcessModel> collection)
        {
            var processes = GetProcess();
            var toadd = new ObservableCollection<ProcessModel>();
            foreach (var processModel in processes)
            {
                var flag = true;
                foreach (var model in collection)
                {
                    if (model.ProcessId == processModel.ProcessId)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    toadd.Add(processModel);
                }

            }
            if (toadd.Count > 0)
            {
                foreach (var processModel in toadd)
                {
                    collection.Add(processModel);
                }
            }
        }
        public static void BeforDeleteProcess(ObservableCollection<ProcessModel> collection)
        {
            var newprocesses = GetProcess();
            var flag = true;
            var todelete = new ObservableCollection<ProcessModel>();
            foreach (var processModel in collection)
            {
                if (!newprocesses.Contains(processModel))
                {
                    todelete.Add(processModel);
                }
            }
            if (todelete.Count > 0)
            {
                foreach (var processModel in todelete)
                {
                    collection.Remove(processModel);
                }
            }
        }
    }
}
