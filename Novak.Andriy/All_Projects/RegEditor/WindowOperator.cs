using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RegEditor
{
    internal static class WindowOperator
    {
        public static void Create_Window(UserControl ctr, string title = "Window", bool isDialog = false)
        {
            var clone = Application.Current.Windows.OfType<Window>().
                FirstOrDefault(w => w.Content.GetType() == ctr.GetType());
            if (clone != null) return;

            var wnd = new Window
            {
                Title = title,
                Content = ctr,
                SizeToContent = SizeToContent.WidthAndHeight,
                Topmost = true
            };
            if (isDialog)
                wnd.ShowDialog();
            else
                wnd.Show();
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
