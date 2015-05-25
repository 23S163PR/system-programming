using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

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
			var priorityName = (sender as MenuItem).Name;
			ProcessPriorityClass res;
			priorityName = priorityName.Replace("PriorityMenuItem", "");
			Enum.TryParse(priorityName, out res);

			_processManager.SetProcessPriority(ProcessDataGrid.GetSelectedProcessId(), res);
		}

		private void ContextMenu_Opening(object sender, RoutedEventArgs e)
		{
			_timer.Stop();

			foreach (MenuItem menuItem in PriorityMenuItem.Items)
			{
				menuItem.IsChecked = false;
			}

			switch (_processManager.GetProcessPriority(ProcessDataGrid.GetSelectedProcessId()))
			{
				case ProcessPriorityClass.RealTime:
					RealtimePriorityMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.High:
					HighPriorityMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.AboveNormal:
					AboveNormalPriorityMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.Normal:
					NormalPriorityMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.BelowNormal:
					BelowNormalPriorityMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.Idle:
					LowPriorityMenuItem.IsChecked = true;
					break;
			}
		}

		private void ContextMenu_OnClosed(object sender, RoutedEventArgs e)
		{
			_timer.Start();
		}
	}
}