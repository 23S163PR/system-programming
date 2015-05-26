using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);

        private const int SW_RESTORE = 0x09;

        public App()
        {
            const string MutexName = "4C98FD71-A38E-4B0C-8F53-675FC8D6649B";
            Mutex mutex = null;
            if (Mutex.TryOpenExisting(MutexName, out mutex))
            {
                var process = Process.GetProcessesByName("WpfApplication1").First(p => p.MainWindowHandle != IntPtr.Zero);
                ShowWindow(process.MainWindowHandle, SW_RESTORE);
                SetForegroundWindow(process.MainWindowHandle);

                Environment.Exit(0);
            }
            else
            {
                mutex = new Mutex(true, MutexName);
            }
        }
    }
}
