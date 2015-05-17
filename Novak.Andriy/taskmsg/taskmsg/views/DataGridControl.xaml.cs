using System;
using System.Diagnostics;
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
			foreach (MenuItem item in PriorityMenu.Items)
			{
				item.IsChecked = false;
			}
			var curent = ProcessGrid.SelectedItem as ProcessModel;
			_tmsg.CurentProcessId = curent == null ? -1 : curent.Id;
			if (_tmsg.CurentProcessId < 0)return;
			try
			{
				switch (_tmsg.GetProcesPriorityClass(_tmsg.CurentProcessId))
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
			catch (Exception){}
		}

		private void ChangePriorityClick(object sender, RoutedEventArgs e)
		{
			var menuItem = sender as MenuItem;
			if (menuItem == null)return;
			var name = menuItem.Header.ToString().Replace(" ", "");
			ProcessPriorityClass res;
			Enum.TryParse(name, out res);
			try
			{
				if (_tmsg.CurentProcessId < 0) return;
			    Taskmsg.SetProcessPrioruty(_tmsg.CurentProcessId, res);
			}
			catch (Exception){}	
		}

		private void KillProcessClick(object sender, RoutedEventArgs e)
		{
			if (_tmsg.CurentProcessId < 0) return;
			Taskmsg.CloseProcess(_tmsg.CurentProcessId);
		}
	}
}
