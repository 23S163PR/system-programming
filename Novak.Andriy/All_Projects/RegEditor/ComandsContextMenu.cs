using System.Windows.Input;

namespace RegEditor
{
    public static class ComandsContextMenu 
    {
        public static readonly RoutedUICommand CreateKey = new RoutedUICommand(
            "Create Key", "CreateKey", typeof(MainWindow));

        public static readonly RoutedUICommand UpdateKey = new RoutedUICommand(
            "Update Key", "UpdateKey", typeof(MainWindow));

        public static readonly RoutedUICommand DeleteKey = new RoutedUICommand(
            "Delete Key", "DeleteKey", typeof(MainWindow));

        public static readonly RoutedUICommand DeleteKeyValue = new RoutedUICommand(
            "Delete Key Value", "DeleteKeyValue", typeof(MainWindow));

        public static readonly RoutedUICommand CreateKeyValue = new RoutedUICommand(
           "Delete Key Value", "DeleteKeyValue", typeof(MainWindow));

        public static readonly RoutedUICommand UpdateKeyValue = new RoutedUICommand(
           "Update Key Value", "UpdateKeyValue", typeof(MainWindow));

    }
}
