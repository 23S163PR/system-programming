using System.Windows;

namespace RegEditor
{
	public partial class MainWindow : Window
	{
		private readonly RegestryEditor _regestryEditor;
		public MainWindow()
		{
			InitializeComponent();
			_regestryEditor = new RegestryEditor();
			foreach (var item in _regestryEditor.Items)
			{
				RegistryKeys.Items.Add(item);
			}
			
		}
	}
}
