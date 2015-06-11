using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TaskManager.Classes
{
    public class ProcessInfo : INotifyPropertyChanged
    {
        private int _processId;
        private String _name;
        private int _numThreads;
        private long _workingSetInB;
        private double _workingSetInKb;
        private double _workingSetInMb;
        private PerformanceCounter _cpuUssingCounter;
        private float _cpuUssingInPercent;

        public int ProcessId
        {
            get { return _processId; }
            private set
            {
                if (_processId == value) return;
                _processId = value;
            }
        }
        public String Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged();
            }
        }
        public int NumThreads
        {
            get { return _numThreads; }
            set
            {
                if (_numThreads == value) return;
                _numThreads = value;
                NotifyPropertyChanged();
            }
        }
        public long WorkingSetInB
        {
            get { return _workingSetInB; }
            set
            {
                if (_workingSetInB == value) return;
                _workingSetInB = value;
                NotifyPropertyChanged();
            }
        }
        public double WorkingSetInKb
        {
            get { return _workingSetInKb; }
            set
            {
                if (_workingSetInKb == value) return;
                _workingSetInKb = value;
                NotifyPropertyChanged();
            }
        }
        public double WorkingSetInMb
        {
            get { return _workingSetInMb; }
            set
            {
                if (_workingSetInMb == value) return;
                _workingSetInMb = value;
                NotifyPropertyChanged();
            }
        }
        public PerformanceCounter CpuUssingCounter
        {
            get { return _cpuUssingCounter; }
            set
            {
                if (_cpuUssingCounter == value) return;
                _cpuUssingCounter = value;
                NotifyPropertyChanged();
            }
        }
        public float CpuUssingInPercent
        {
            get { return _cpuUssingInPercent; }
            set
            {
                if (_cpuUssingInPercent == value) return;
                _cpuUssingInPercent = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public ProcessInfo(int processId
                            , String name
                            , int numThreads
                            , long workingSetInB
                            , PerformanceCounter cpuUssingCounter)
        {
            ProcessId = processId;
            Name = name;
            NumThreads = numThreads;
            WorkingSetInB = workingSetInB;
            WorkingSetInKb = WorkingSetInB / 1024;
            WorkingSetInMb = WorkingSetInKb / 1024;
            CpuUssingCounter = cpuUssingCounter;
            CpuUssingInPercent = CpuUssingCounter.NextValue();
        }

        public void UpdateInfo( int numThreads
                                , long workingSetInB)
        {
            NumThreads = numThreads;
            WorkingSetInB = workingSetInB;
            WorkingSetInKb = WorkingSetInB / 1024;
            WorkingSetInMb = WorkingSetInKb / 1024;
            CpuUssingInPercent = CpuUssingCounter.NextValue();
        }
        public String OutputFormatData<T>(T data)
        {
            return String.Format("{0, 3}", data);
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
