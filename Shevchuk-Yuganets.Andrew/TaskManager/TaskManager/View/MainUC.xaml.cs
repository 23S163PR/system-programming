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

		public MainUC()
		{
			InitializeComponent();
			_timer = new DispatcherTimer();
			_timer.Tick += timer_Tick;
			_timer.Interval = new TimeSpan(0, 0, 3);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			ProcessDataGrid.DataContext = MainControl.GetProcessList();
			_timer.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			string dataGridSortDescription = null;
			ListSortDirection? dataGridSortDirection = null;
			var columnIndex = 0;

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
				var collectionView = (CollectionView)CollectionViewSource.GetDefaultView(ProcessDataGrid.ItemsSource);
				collectionView.SortDescriptions.Add(sortDescription);
				ProcessDataGrid.Columns[columnIndex].SortDirection = dataGridSortDirection;
			}
		}

		private void EndProces_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Kill Process?", "Kill Process", MessageBoxButton.OKCancel, MessageBoxImage.Warning) ==
			    MessageBoxResult.OK)
			{
				MainControl.KillProcess(ProcessDataGrid.GetSelectedProcessId());
            }
		}

		private void Priority_Click(object sender, RoutedEventArgs e)
		{
			var processPriorityClass = ProcessPriorityClass.Normal;
			// need better idea
			switch ((sender as MenuItem).Name)
			{
				case "RealTime":
					processPriorityClass = ProcessPriorityClass.RealTime;
					break;
				case "High":
					processPriorityClass = ProcessPriorityClass.High;
					break;
				case "Above Normal":
					processPriorityClass = ProcessPriorityClass.AboveNormal;
					break;
				case "Normal":
					processPriorityClass = ProcessPriorityClass.Normal;
					break;
				case "Below Normal":
					processPriorityClass = ProcessPriorityClass.BelowNormal;
					break;
				case "Low":
					processPriorityClass = ProcessPriorityClass.Idle;
					break;
			}

			MainControl.SetProcessPriority(ProcessDataGrid.GetSelectedProcessId(), processPriorityClass);
		}

		private void ContextMenu_Opening(object sender, RoutedEventArgs e)
		{
			// xaml default value false
			RealtimeMenuItem.IsChecked = false;
			HighMenuItem.IsChecked = false;
			AboveNormalMenuItem.IsChecked = false;
			NormalMenuItem.IsChecked = false;
			BelowNormalMenuItem.IsChecked = false;
			LowMenuItem.IsChecked = false;

			switch (MainControl.GetProcessPriority(ProcessDataGrid.GetSelectedProcessId()))
			{
				case ProcessPriorityClass.RealTime:
					RealtimeMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.High:
					HighMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.AboveNormal:
					AboveNormalMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.Normal:
					NormalMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.BelowNormal:
					BelowNormalMenuItem.IsChecked = true;
					break;
				case ProcessPriorityClass.Idle:
					LowMenuItem.IsChecked = true;
					break;
			}
		}
	}
}