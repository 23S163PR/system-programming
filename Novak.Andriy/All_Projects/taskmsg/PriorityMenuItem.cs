using System.Diagnostics;
using System.Windows.Controls;

namespace ProcessManager
{
	internal class PriorityMenuItem : MenuItem
	{
		public ProcessPriorityClass PriorityValue { get; set; }
	}
}