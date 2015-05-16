using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            ListProcesses.ItemsSource = Manager.GetProcess();
            InitializeTimer();
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
            Manager.UpdateProcessesInfo(ListProcesses);
            if (Manager.CheckToAddNewProcess(ListProcesses))
            {
                ListProcesses.ItemsSource = Manager.GetProcess();
            }
            ListProcesses.Items.Refresh();
        }
        private void CloseProcess_Button_OnClick(object sender, RoutedEventArgs e)
        {
            var process = ListProcesses.SelectedItem as ProcessModel;
            if (process != null)
            {
                var a = Process.GetProcessById(process.ProcessId);
                try
                {
                    a.Kill();
                }
                catch (Exception)
                {
                    // ignored
                }
                ListProcesses.ItemsSource = Manager.GetProcess();
            }
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            ListProcesses.ItemsSource = Manager.GetProcess();
            ListProcesses.Items.Refresh();
        }
    }
}
