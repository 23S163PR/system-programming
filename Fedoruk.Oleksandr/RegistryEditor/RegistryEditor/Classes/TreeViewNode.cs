using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Security;
using System.Text.RegularExpressions;

namespace RegistryEditor.Classes
{
    public class TreeViewNode
    {
        public String Path { get; set; }
        public String Name { get; set; }
        public Nullable<RegistryHive> RootValue { get; set; }
        public ObservableCollection<TreeViewNode> Items { get; set; }
        public ObservableCollection<RegistryKeyValue> Values { get; set; }


        public TreeViewNode(String path, String name)
        {
            Items = new ObservableCollection<TreeViewNode>();
            Values = new ObservableCollection<RegistryKeyValue>();
            if (Regex.IsMatch(path, @"(HKEY)\w+\\\\"))
                path = Regex.Replace(path, @"(HKEY)\w+\\\\", "");
            else if (Regex.IsMatch(path, @"(HKEY)\w+\\"))
                path = Regex.Replace(path, @"(HKEY)\w+\\", "");
            Path = path;
            Name = name;
        }
        public TreeViewNode(String path, String name, RegistryHive rootValue)
            : this(path, name)
        {
            RootValue = rootValue;
        }

        public void AddSubNodes()
        {
            if (RootValue == null) return;
            var rootKey = RegistryKey.OpenBaseKey(RootValue.Value, Environment.Is64BitOperatingSystem
                                                                                ? RegistryView.Registry64
                                                                                : RegistryView.Registry32);

            var key = rootKey.OpenSubKey(Path);
            foreach (var el in key.GetSubKeyNames())
            {
                try
                {
                    var subKey = key.OpenSubKey(el);
                    var path = subKey.Name;
                    Items.Add(new TreeViewNode(path, el, RootValue.Value));
                    subKey.Close();
                }
                catch (SecurityException)
                {
                    continue;
                }
            }
        }

        public void AddValues()
        {
            if (RootValue == null) return;
            var rootKey = RegistryKey.OpenBaseKey(RootValue.Value, Environment.Is64BitOperatingSystem
                                                                                ? RegistryView.Registry64
                                                                                : RegistryView.Registry32);

                var key = rootKey.OpenSubKey(Path);
                foreach (var el in key.GetValueNames())
                {
                    var type = key.GetValueKind(el);
                    Values.Add(new RegistryKeyValue(el, type.ToString(), "aa"));
                }

        }

        public override string ToString()
        {
            return Name;
        }
    }
}
