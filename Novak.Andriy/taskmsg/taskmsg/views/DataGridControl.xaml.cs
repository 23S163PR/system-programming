using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace taskmsg.views
{
	

	public partial class DataGridControl : UserControl
	{
		private readonly Taskmsg _tmsg;

		public DataGridControl()
		{
			InitializeComponent();
			_tmsg = new Taskmsg();
			ProcessGrid.DataContext = _tmsg.Processes;
			var dispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
			dispatcherTimer.Tick += OnDispatcherTimerOnTick;
			dispatcherTimer.Start();	
		}

		private void OnDispatcherTimerOnTick(object sender, EventArgs args)
		{

			_tmsg.RefreshProcesses();
			ProcessGrid.DataContext = _tmsg.Processes;
			LbCount.Content = string.Format("Processes: {0}", _tmsg.Processes.Count());
			ProcessGrid.Items.Refresh();
		}

		private void TaskContextMenu_OnOpened(object sender, RoutedEventArgs e)
		{
			var curent = ProcessGrid.SelectedItem as ProcessModel;
			if (curent == null) return;
			_tmsg.CurentProcessId = curent.Id;
		}
	}
}
