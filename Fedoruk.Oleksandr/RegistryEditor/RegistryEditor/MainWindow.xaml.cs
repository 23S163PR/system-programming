using RegistryEditor.Classes;
using System;
using System.Threading;
using System.Threading.Tasks;
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

            Parallel.ForEach(expandedNode.Items, el => 
            {
                if (el.Items.Count == 0)
                {
                    el.AddSubNodes();
                }
            });
            AddValuesInNode(expandedNode);
        }

        private void treeView_Selected_1(object sender, RoutedEventArgs e)
        {
            var expandedItem = (TreeViewItem)e.OriginalSource;
            var expandedNode = (TreeViewNode)expandedItem.Header;
            AddValuesInNode(expandedNode);
        }

        private void AddValuesInNode(TreeViewNode node)
        {
            if (node.Values.Count == 0)
            {
                node.AddValues();
            }
            dGrid.ItemsSource = node.Values;
        }
    }
}
