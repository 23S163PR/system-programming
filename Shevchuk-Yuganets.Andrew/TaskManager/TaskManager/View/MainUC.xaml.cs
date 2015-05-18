using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using TaskManager.Control;

namespace TaskManager.View
{
	/// <summary>
	///     Interaction logic for MainUC.xaml
	/// </summary>
	public partial class MainUC : UserControl
	{
		private readonly DispatcherTimer _timer;
		private int _selectedProcessId;

		public MainUC()
		{
			InitializeComponent();
			_timer = new DispatcherTimer();
			_timer.Tick += timer_Tick;
			_timer.Interval = new TimeSpan(0, 0, 1);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			_timer.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			string dataGridSortDescription = null;
			ListSortDirection? dataGridSortDirection = null;
			var columnIndex = 0;

			var activeRow = ProcessDataGrid.SelectedValue;
			var activeColumn = ProcessDataGrid.Columns.FirstOrDefault(col => col.SortDirection != null);
			if (activeColumn != null)
			{
				dataGridSortDirection = activeColumn.SortDirection;
				dataGridSortDescription = activeColumn.SortMemberPath;
				columnIndex = activeColumn.DisplayIndex;
			}

			ProcessDataGrid.DataContext = MainControl.GetProcessList();

			if (dataGridSortDirection != null)
			{
				var sortDescription = new SortDescription(dataGridSortDescription, dataGridSortDirection.Value);
				var collectionView = (CollectionView) CollectionViewSource.GetDefaultView(ProcessDataGrid.ItemsSource);
				collectionView.SortDescriptions.Add(sortDescription);
				ProcessDataGrid.Columns[columnIndex].SortDirection = dataGridSortDirection;
			}

			if (activeRow != null)
			{
				ProcessDataGrid.SelectedValue = activeRow;
			}
		}

		private void EndProcess_Click(object sender, RoutedEventArgs e)
		{
			_selectedProcessId = ProcessDataGrid.GetSelectedProcessId();
			if (_selectedProcessId < 0)
			{
				return;
			}

			if (MessageBox.Show("Kill Process?", "Kill Process", MessageBoxButton.OKCancel, MessageBoxImage.Warning) ==
			    MessageBoxResult.OK)
			{
				MainControl.KillProcess(_selectedProcessId);
			}
		}

		private void ChangePriority_Click(object sender, RoutedEventArgs e)
		{
			_selectedProcessId = ProcessDataGrid.GetSelectedProcessId();
			if (_selectedProcessId < 0)
			{
				return;
			}

			var priorityName = (sender as MenuItem).Name;
			ProcessPriorityClass res;
			priorityName = priorityName.Replace("PriorityMenuItem", "");
			Enum.TryParse(priorityName, out res);

			MainControl.SetProcessPriority(_selectedProcessId, res);
		}

		private void ContextMenu_Opening(object sender, RoutedEventArgs e)
		{
			_timer.Stop();

			_selectedProcessId = ProcessDataGrid.GetSelectedProcessId();
			if (_selectedProcessId < 0)
			{
				return;
			}

			foreach (MenuItem menuItem in PriorityMenuItem.Items)
			{
				menuItem.IsChecked = false;
			}

			switch (MainControl.GetProcessPriority(_selectedProcessId))
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