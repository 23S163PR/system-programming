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
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
            //ListProcesses.Items.SortDescriptions.Add(new SortDescription());
            // Add items to ItemsSource in ListView
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListProcesses.ItemsSource);
            //view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            // Add sorting by ascending with field "Name"
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ListProcesses.ItemsSource = Manager.GetProcess();
        }


        private void CloseProcess_Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListProcesses.SelectedItem is ProcessModel)
            {
                var a = Process.GetProcessById((ListProcesses.SelectedItem as ProcessModel).ProcID);
                a.Kill();
            }
        }

        public void Refresh(object o, RoutedEventArgs e)
        {
            var selectedproc = ListProcesses.SelectedItems;
            ListProcesses.ItemsSource = Manager.GetProcess();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListProcesses.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
    }
}
