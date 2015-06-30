using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace ProcessManager.views
{
	public partial class DataGridControl : UserControl
	{
	    private readonly TaskManager _taskManager;
	    private readonly Timer _timer;
		public DataGridControl()
		{
			InitializeComponent();
			_taskManager = new TaskManager();
            _timer = new Timer(1000); // 1000ms - 1sec
			ProcessGrid.DataContext = _taskManager.Processes;
            _timer.Elapsed += TimerOnTick;
            _timer.Enabled = true;
            _timer.Start();
		}

		private void TimerOnTick(object sender, EventArgs args)
		{
            _taskManager.RefreshProcesses();
			Application.Current.Dispatcher.Invoke(()=>LbCount.Content
                = string.Format("Processes: {0}", _taskManager.Processes.Count()));
		}

		private void TaskContextMenu_OnOpened(object sender, RoutedEventArgs e)
		{
            _timer.Stop();
			var curent = ProcessGrid.SelectedItem as ProcessModel;
            _taskManager.CurentProcessId = curent == null ? -1 : curent.Id;
            if (_taskManager.CurentProcessId < 0) return;

		    try
		    {
		        var priority = _taskManager.GetProcesPriorityClass(_taskManager.CurentProcessId);
                foreach (PriorityMenuItem menuItem in PriorityMenu.Items)
                {
                    menuItem.IsChecked = menuItem.PriorityValue == priority;
                }
		    }
		    catch (Win32Exception ex)
		    {
		        MessageBox.Show(ex.Message, "Access Denied!", MessageBoxButton.OK, MessageBoxImage.Error);
		    }           
		}

		private void ChangePriorityClick(object sender, RoutedEventArgs e)
		{	
			if (_taskManager.CurentProcessId < 0) return;
		    try
		    {
		        TaskManager.SetProcessPrioruty(_taskManager.CurentProcessId, ((PriorityMenuItem) sender).PriorityValue);
		    }
		    catch (Exception ex)
		    {
		        MessageBox.Show(ex.Message);
		    }            
		}

		private void KillProcessClick(object sender, RoutedEventArgs e)
		{
			if (_taskManager.CurentProcessId < 0) return;
			_taskManager.CloseProcess(_taskManager.CurentProcessId);
		}

	    private void TaskContextMenu_OnClosed(object sender, RoutedEventArgs e)
	    {
            _timer.Start();
	    }
	}
}
