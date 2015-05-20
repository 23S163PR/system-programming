using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager
{
    public class ProcessModel : INotifyPropertyChanged
    {
        private string _name;
        private int _thread;
        private string _memory;
        private string _cpu;
        private int _processid;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;

                _name = value;
                InvokePropertyChanged();
            }
        }
        public int Thread
        {
            get { return _thread; }
            set
            {
                if (_thread == value) return;

                _thread = value;
                InvokePropertyChanged();
            }
        }
        public string Memory
        {
            get { return _memory; }
            set
            {
                if (_memory == value) return;

                _memory = value;
                InvokePropertyChanged();
            }
        }
        public string Cpu
        {
            get { return _cpu; }
            set
            {
                if (_cpu == value) return;

                _cpu = value;
                InvokePropertyChanged();
            }
        }
        public int ProcessId
        {
            get { return _processid; }
            set
            {
                if (_processid == value) return;

                _processid = value;
                InvokePropertyChanged();
            }
        }

        public ProcessModel(string name, int thread, float memory, TimeSpan cpu, int processId)
        {
            Name = name;
            Thread = thread;
            Cpu = cpu.Milliseconds.ToString("F1");
            ProcessId = processId;
            Memory = ConvertMemory.ShorteningMemory(memory);
        }
        public ProcessModel(Process process)
        {
            Name = process.ProcessName;
            Thread = process.Threads.Count;
            try
            {
                Cpu = process.UserProcessorTime.Milliseconds.ToString("F1");
            }
            catch (Exception)
            {
                Cpu = "none";
            }

            ProcessId = process.Id;
            Memory = ConvertMemory.ShorteningMemory(process.WorkingSet64);
        }
        public void SetInfoProcess(int thread, float memory, TimeSpan cpu)
        {
            Thread = thread;
            Cpu = cpu.Milliseconds.ToString("F1");
            Memory = ConvertMemory.ShorteningMemory(memory);
        }
        public void SetInfoProcess(Process process)
        {
            Thread = process.Threads.Count;
            try
            {
                Cpu = process.UserProcessorTime.Milliseconds.ToString();
            }
            catch (Exception)
            {
                Cpu = "none";
            }
            Memory = ConvertMemory.ShorteningMemory(process.WorkingSet64);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void InvokePropertyChanged([CallerMemberName] string member = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(member));
            }
        }
    }
}
