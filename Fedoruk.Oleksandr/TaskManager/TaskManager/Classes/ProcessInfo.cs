using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TaskManager.Classes
{
    public class ProcessInfo : INotifyPropertyChanged
    {
        private int _numThreads;
        private long _workingSetInB;
        private double _workingSetInKb;
        private double _workingSetInMb;
        private int _cpuUssingInPercent;

        public int ProcessId
        {
            get;
            private set;
        }
        public String Name
        {
            get;
            private set;
        }
        public int NumThreads
        {
            get { return _numThreads; }
            private set
            {
                if (_numThreads == value) return;
                _numThreads = value;
                NotifyPropertyChanged();
            }
        }
        public long WorkingSetInB
        {
            get { return _workingSetInB; }
            private set
            {
                if (_workingSetInB == value) return;
                _workingSetInB = value;
                NotifyPropertyChanged();
            }
        }
        public double WorkingSetInKb
        {
            get { return _workingSetInKb; }
            private set
            {
                if (_workingSetInKb == value) return;
                _workingSetInKb = value;
                NotifyPropertyChanged();
            }
        }
        public double WorkingSetInMb
        {
            get { return _workingSetInMb; }
            private set
            {
                if (_workingSetInMb == value) return;
                _workingSetInMb = value;
                NotifyPropertyChanged();
            }
        }
        public PerformanceCounter CpuUssingCounter
        {
            get;
            private set;
        }
        public int CpuUssingInPercent
        {
            get { return _cpuUssingInPercent; }
            private set
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
            CpuUssingCounter = cpuUssingCounter;
            UpdateInfo(numThreads, workingSetInB);
        }

        public void UpdateInfo( int numThreads
                                , long workingSetInB)
        {
            NumThreads = numThreads;
            WorkingSetInB = workingSetInB;
            WorkingSetInKb = WorkingSetInB / 1024;
            WorkingSetInMb = WorkingSetInKb / 1024;
            CpuUssingInPercent = (int)(CpuUssingCounter.NextValue() / Environment.ProcessorCount);
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
