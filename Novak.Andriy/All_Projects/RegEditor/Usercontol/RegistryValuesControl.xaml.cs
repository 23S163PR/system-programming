using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace RegEditor.Usercontol
{
    public partial class RegistryValuesControl : UserControl
    {
        private readonly IEnumerable<string> _typesKind = Enum.GetNames(typeof (RegistryValueKind));
        public RegistryValue RegistryValue { get; private set; }

        public bool DialogResult { get; private set; }

        public RegistryValuesControl()
        {
            InitializeComponent();
            cbKeyType.ItemsSource = _typesKind;
            cbKeyType.SelectedIndex = 0;
        }

        public RegistryValuesControl(RegistryValue? value)
        {
            InitializeComponent();
            tbKeyName.Text = value.Value.Name;
            cbKeyType.ItemsSource = _typesKind;
            cbKeyType.SelectedValue = value.Value.Type.ToString();
            tbKeyValue.Text = value.Value.Value.ToString();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if(!tbKeyValue.Text.Any() || !tbKeyName.Text.Any())return;
            var type = (RegistryValueKind)Enum.Parse(typeof(RegistryValueKind), cbKeyType.SelectedValue.ToString());
            RegistryValue = new RegistryValue
            {
                Name = tbKeyName.Text
                ,Value = tbKeyValue.Text
                ,Type = type
            };
            DialogResult = true;
            WindowOperator.Cancel_Click(this);
        }
    }
}
