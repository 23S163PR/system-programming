using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ProcessModel> _processes;
        public ObservableCollection<ProcessModel> Processes
        {
            get { return _processes; }
        }
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            InitializeProcesses();
            this.DataContext = this;
        }
        public void InitializeProcesses()
        {
            _processes = new ObservableCollection<ProcessModel>();
            foreach (var processModel in Manager.GetProcess())
            {
                Processes.Add(processModel);
            } 
        }
        public void InitializeTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Manager.UpdateProcessesInfo(Processes);
            Manager.AddNewProcesses(Processes);
            //Manager.BeforDeleteProcess(Processes);
        }
        private void CloseProcess_Button_OnClick(object sender, RoutedEventArgs e)
        {
            var process = ListProcesses.SelectedItem as ProcessModel;
            if (process != null)
            {
                var a = Process.GetProcessById(process.ProcessId);
                try
                {
                    var todel = new ObservableCollection<ProcessModel>();
                    foreach (var processModel in Processes)
                    {
                        if (processModel.Name == a.ProcessName)
                        {
                            todel.Add(processModel);
                        }
                    }
                    if (todel.Count > 0)
                    {
                        foreach (var processModel in todel)
                        {
                            Processes.Remove(processModel);
                        }
                    }
                    a.Kill();
                }
                catch (Exception)
                {
                     //ignored
                }
            }
        }
        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            Manager.BeforDeleteProcess(Processes);   
        }
    }
}
