using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RegEditor.Usercontol
{
    public partial class RegistryKeyControl : UserControl
    {
        public bool DialogResult { get; private set; }
        public string KeyName { get; private set; }
        public RegistryKeyControl()
        {
            InitializeComponent();
        }

        public RegistryKeyControl(string keyName)
        {
            InitializeComponent();
            tbKeyName.Text = keyName;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if(!tbKeyName.Text.Any())return;
            KeyName = tbKeyName.Text;
            DialogResult = true;
            WindowOperator.Cancel_Click(this);
        }
    }
}
