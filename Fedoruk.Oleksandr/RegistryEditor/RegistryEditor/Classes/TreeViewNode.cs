using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace RegistryEditor.Classes
{
    public class TreeViewNode
    {
        public String Path { get; private set; }
        public String Name { get; private set; }
        public Nullable<RegistryHive> RootValue { get; private set; }
        public ObservableCollection<TreeViewNode> Items { get; private set; }
        public ObservableCollection<RegistryKeyValue> Values { get; private set; }


        public TreeViewNode(String path, String name)
        {
            Items = new ObservableCollection<TreeViewNode>();
            Values = new ObservableCollection<RegistryKeyValue>();
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
                    if (subKey == null) continue;
                    var path = Regex.Replace(subKey.Name, @"^(HKEY)\w+\\{1,2}", "");
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
                var value = GetValueInString(type, key.GetValue(el));
                var name = el == "" ? "{Default}" : el;
                Values.Add(new RegistryKeyValue(name, type.ToString(), value));
            }
        }

        public String GetValueInString(RegistryValueKind type, object value)
        {
            String valueInString = "";
            switch (type)
            {
                case RegistryValueKind.MultiString:
                    string[] values = (string[])value;
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i != 0) valueInString += ",";
                        valueInString += values[i];
                    }
                    break;

                case RegistryValueKind.Binary:
                    byte[] bytes = (byte[])value;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        valueInString += String.Format(" {0:X2}", bytes[i]);
                    }
                    break;

                default:
                    valueInString = value.ToString();
                    break;
            }
            return valueInString;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
