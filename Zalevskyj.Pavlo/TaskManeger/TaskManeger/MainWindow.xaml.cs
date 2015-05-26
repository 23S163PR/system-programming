using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TaskManeger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer _dispatcherTimer;
        TaskManager _taskManaker;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            _taskManaker = new TaskManager();

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();

            ProcessDataGrid.DataContext = _taskManaker.GetProcessList();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //ProcessDataGrid.DataContext = _taskManaker.GetProcessList();
            ProcessDataGrid.Items.Refresh();
        }

        private void EndProcess_Click_1(object sender, RoutedEventArgs e)
        {
            _taskManaker.EndProcess((ProcessDataGrid.SelectedItem as SystemProcess).Id);
        }

        private void ContextMenu_Opened_1(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();

            string nameMenuItem = "";
            switch (_taskManaker.GetPrioruteProcess((ProcessDataGrid.SelectedItem as SystemProcess).Id))
            {
                case ProcessPriorityClass.RealTime:
                    nameMenuItem = "Realtime";
                    break;
                case ProcessPriorityClass.High:
                    nameMenuItem = "High";
                    break;
                case ProcessPriorityClass.AboveNormal:
                    nameMenuItem = "Above Normal";
                    break;
                case ProcessPriorityClass.Normal:
                    nameMenuItem = "Hormal";
                    break;
                case ProcessPriorityClass.BelowNormal:
                    nameMenuItem = "Below Normal";
                    break;
                case ProcessPriorityClass.Idle:
                    nameMenuItem = "Low";
                    break;
            }

            foreach (var item in menuPriority.Items)
            {
                if ((item as MenuItem).Header.ToString() == nameMenuItem)
                {
                    (item as MenuItem).IsChecked = true;
                }
            }
        }

        private void ContextMenu_Closed_1(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Start();
        }
        private void priority_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in menuPriority.Items)
            {
                (item as MenuItem).IsChecked = false;
            }
            switch ((sender as MenuItem).Header.ToString())
            {
                case "Real time":
                    _taskManaker.ChangepPriority((ProcessDataGrid.SelectedItem as SystemProcess).Id
                        , ProcessPriorityClass.RealTime);
                    break;
                case "High":
                    _taskManaker.ChangepPriority((ProcessDataGrid.SelectedItem as SystemProcess).Id
                        , ProcessPriorityClass.High);
                    break;
                case "Above Normal":
                    _taskManaker.ChangepPriority((ProcessDataGrid.SelectedItem as SystemProcess).Id
                        , ProcessPriorityClass.AboveNormal);
                    break;
                case "Hormal":
                    _taskManaker.ChangepPriority((ProcessDataGrid.SelectedItem as SystemProcess).Id
                        , ProcessPriorityClass.Normal);
                    break;
                case "Below Normal":
                    _taskManaker.ChangepPriority((ProcessDataGrid.SelectedItem as SystemProcess).Id
                        , ProcessPriorityClass.BelowNormal);
                    break;
                case "Low":
                    _taskManaker.ChangepPriority((ProcessDataGrid.SelectedItem as SystemProcess).Id
                        , ProcessPriorityClass.Idle);
                    break;
            }
        }
    }
}
