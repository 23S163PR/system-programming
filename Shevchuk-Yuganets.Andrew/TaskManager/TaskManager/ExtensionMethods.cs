using System.Windows.Controls;
using TaskManager.Model;

namespace TaskManager
{
	public static class DataGridExtension
	{
		public static int GetSelectedProcessId(this DataGrid dataGrid)
		{
			return (dataGrid.SelectedItem as ProcessModel).ProcessId;
		}
	}
}
