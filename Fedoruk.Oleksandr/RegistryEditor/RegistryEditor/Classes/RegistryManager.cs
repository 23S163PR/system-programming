using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;

namespace RegistryEditor.Classes
{
    public class RegistryManager
    {
        public ObservableCollection<TreeViewNode> TreeViewNodes { get; private set; }


        public RegistryManager()
        {
            TreeViewNodes = new ObservableCollection<TreeViewNode>();
            InitData();
        }

        public void InitData()
        {
            var rootNode = new TreeViewNode("", "Computer");
            TreeViewNodes.Add(rootNode);

            rootNode.Items.Add(new TreeViewNode("", "HKEY_CLASSES_ROOT", RegistryHive.ClassesRoot));
            rootNode.Items.Add(new TreeViewNode("", "HKEY_CURRENT_USER", RegistryHive.CurrentUser));
            rootNode.Items.Add(new TreeViewNode("", "HKEY_LOCAL_MACHINE", RegistryHive.LocalMachine));
            rootNode.Items.Add(new TreeViewNode("", "HKEY_USERS", RegistryHive.Users));
            rootNode.Items.Add(new TreeViewNode("", "HKEY_CURRENT_CONFIG", RegistryHive.CurrentConfig));
            foreach (var el in rootNode.Items)
            {
                el.AddSubNodes();
            }
        }
    }
}
