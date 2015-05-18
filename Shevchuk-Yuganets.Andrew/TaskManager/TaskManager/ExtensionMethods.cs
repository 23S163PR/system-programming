using System.Windows.Controls;
using TaskManager.Model;

namespace TaskManager
{
	public static class DataGridExtension
	{
		public static int GetSelectedProcessId(this DataGrid dataGrid)
		{
			var process = dataGrid.SelectedItem as ProcessModel;
            return (process == null)? -1  : process.ProcessId;
		}
	}
}
