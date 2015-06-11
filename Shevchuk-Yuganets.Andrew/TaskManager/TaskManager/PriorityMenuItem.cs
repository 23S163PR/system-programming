using System.Diagnostics;
using System.Windows.Controls;

namespace TaskManager
{
	internal class PriorityMenuItem : MenuItem
	{
		public ProcessPriorityClass PriorityValue { get; set; }
	}
}