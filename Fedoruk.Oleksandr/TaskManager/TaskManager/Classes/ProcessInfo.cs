using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Classes
{
    public class ProcessInfo : INotifyPropertyChanged
    {
        private int _threadsCount;
        private float _workingSetKb;
        private float _workingSetMb;
        private PerformanceCounter _CPUUssing;


        public int ProcessId { get; set; }
        public String Name { get; set; } 
        public int ThreadsCount
        {
            get
            {
                return _threadsCount;
            }
            set
            {
                if (_threadsCount == value) return;

                _threadsCount = value;
                InvokePropertyChanged();
            }
            
        }
        
        public float WorkingSetKb
        {
            get
            {
                return _workingSetKb;
            }
            set
            {
                if (_workingSetKb == value) return;

                _workingSetKb = value;
                InvokePropertyChanged();
            }
            
        }
        public float WorkingSetMb
        {
            get
            {
                return _workingSetMb;
            }
            set
            {
                if (_workingSetMb == value) return;

                _workingSetMb = value;
                InvokePropertyChanged();
            }

        }
        public PerformanceCounter CPUUssing
        {
            get
            {
                return _CPUUssing;
            }
            set
            {
                if (_CPUUssing == value) return;

                _CPUUssing = value;
                InvokePropertyChanged();
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;


        public String WorkingSetKbFormat
        {
            get
            {
                return WorkingSetKb.ToString("F2");
            }
        }
        public String WorkingSetMbFormat
        {
            get
            {
                return WorkingSetMb.ToString("F2");
            }
        }
        public String CPUUssingFormat
        {
            get
            {
                try
                {
                    return CPUUssing.NextValue().ToString("F1");
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public ProcessInfo( int id
                            , String name
                            , int NThreads
                            , long WorkingSet
                            , PerformanceCounter CpuUssing)
        {
            ProcessId = id;
            Name = name;
            ThreadsCount = NThreads;
            WorkingSetKb = WorkingSet / 1024;
            WorkingSetMb = WorkingSetKb / 1024;
            _CPUUssing = CpuUssing;

            _CPUUssing.NextValue();
        }

        public void UpdateData( int NThreads
                                , long WorkingSetB)
        {
            ThreadsCount = NThreads;
            WorkingSetKb = WorkingSetB / 1024;
            WorkingSetMb = WorkingSetKb / 1024;
        }


        private void InvokePropertyChanged([CallerMemberName] string member = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(member));
            }
        }
        
    }
}
