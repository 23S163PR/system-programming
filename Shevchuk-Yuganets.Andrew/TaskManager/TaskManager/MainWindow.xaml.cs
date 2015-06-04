using System;
using System.Timers;
using System.Windows;

namespace TaskManager
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly Timer _timer;
		private readonly ProcessManager _processManager;

		public MainWindow()
		{
			InitializeComponent();

			_processManager = new ProcessManager();

			_timer = new Timer(1000); // 1000ms - 1sec
            _timer.Elapsed += timer_Tick;
			_timer.Enabled = true;
		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			ProcessDataGrid.ItemsSource = _processManager.ProcessList;

			_timer.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
            _processManager.UpdateList();
        }

		private void EndProcess_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Kill Process?", "Kill Process", MessageBoxButton.OKCancel, MessageBoxImage.Warning) ==
				MessageBoxResult.OK)
			{
				_processManager.KillProcess(ProcessDataGrid.GetSelectedProcessId());
			}
		}

		private void ChangePriority_Click(object sender, RoutedEventArgs e)
		{
			// This is guaranteed to be PriorityMenuItem
			var priorityMenuItem = (PriorityMenuItem)sender;
			_processManager.SetProcessPriority(ProcessDataGrid.GetSelectedProcessId(), priorityMenuItem.PriorityValue);
		}

		private void ContextMenu_Opening(object sender, RoutedEventArgs e)
		{
			_timer.Stop();

			var currentPriority = _processManager.GetProcessPriority(ProcessDataGrid.GetSelectedProcessId());
            foreach (PriorityMenuItem menuItem in PriorityMenuItem.Items)
			{
				menuItem.IsChecked = menuItem.PriorityValue == currentPriority;
			}
		}

		private void ContextMenu_OnClosed(object sender, RoutedEventArgs e)
		{
			_timer.Start();
		}
	}
}