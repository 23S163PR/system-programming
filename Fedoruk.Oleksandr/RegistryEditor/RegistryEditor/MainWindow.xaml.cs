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
            if(expandedNode.Values.Count == 0)
            {
                expandedNode.AddValues();
            }
            dGrid.ItemsSource = expandedNode.Values;
        }

        private void treeView_Selected_1(object sender, RoutedEventArgs e)
        {
            var expandedItem = (TreeViewItem)e.OriginalSource;
            var expandedNode = (TreeViewNode)expandedItem.Header;
            if (expandedNode.Values.Count == 0)
            {
                expandedNode.AddValues();
            }
            dGrid.ItemsSource = expandedNode.Values;
        }

    }
}
