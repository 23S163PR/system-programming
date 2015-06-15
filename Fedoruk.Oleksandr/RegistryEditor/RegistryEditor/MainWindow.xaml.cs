using RegistryEditor.Classes;
using System.Windows;
using System.Windows.Controls;

namespace RegistryEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RegistryManager _manager;
        public MainWindow()
        {
            InitializeComponent();
            _manager = new RegistryManager();
            this.DataContext = this;
            treeView.ItemsSource = _manager.TreeViewNodes;
        }


        private void treeView_Expanded(object sender, RoutedEventArgs e)
        {
            var expandedItem = (TreeViewItem)e.OriginalSource;
            var expandedNode = (TreeViewNode)expandedItem.Header;

            foreach (var el in expandedNode.Items)
            {
                if (el.Items.Count == 0)
                {
                    el.AddSubNodes();
                }
            }
        }
    }
}
