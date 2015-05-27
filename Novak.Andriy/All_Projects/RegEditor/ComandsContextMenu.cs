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
    }
}
