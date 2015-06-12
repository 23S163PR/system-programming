using System.Timers;
using System.Windows;
using TaskManager.Classes;
namespace TaskManager
{
    /// <summary>
    /// Interaction logic for ProcessManagerWindow.xaml
    /// </summary>
    public partial class ProcessManagerWindow : Window
    {
        private ProcessManager _processManager;
        private Timer _timer;


        public ProcessManagerWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            _processManager = new ProcessManager();
            dGrid.ItemsSource = _processManager.Processes;

            _timer = new Timer(1000);
            _timer.Elapsed += (s, e) => _processManager.UpdateProcessesInfo();
            _timer.Enabled = true;
            _timer.Start();
        }
    }
}
