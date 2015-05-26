using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskManeger
{
    public class SystemProcess : INotifyPropertyChanged
    {
        private int    _id;
        private string _name;
        private string _threads; 
        private string _memory;
        private string _cpu;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;

                _id = value;
                InvokePropertyChanged();
            }
        }
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
        public string Threads
        {
            get { return _threads; }
            set
            {
                if (_threads == value) return;

                _threads = value;
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
        public string CPU
        {
            get { return _cpu; }
            set
            {
                if (_cpu == value) return;

                _cpu = value;
                InvokePropertyChanged();
            }
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
