using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RegEditor
{
    internal static class WindowOperator
    {
        public static void Create_Window(UserControl ctr, string title = "Window")
        {
            var wnd = new Window
            {
                Title = title,
                Content = ctr,
                SizeToContent = SizeToContent.WidthAndHeight,
                Topmost = true
            };
            
            wnd.ShowDialog();
           
        }

        public static bool? Cancel_Click(UserControl ctr)
        {
            var wnd = Window.GetWindow(ctr);
            if (wnd == null) return false;
            wnd.Close();
            return wnd.DialogResult;
        }
    }
}
