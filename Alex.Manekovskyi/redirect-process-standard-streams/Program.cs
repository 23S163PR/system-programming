using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redirect_process_standard_streams
{
	/// <summary>
	/// This sample shows how parent process could
	/// intercept write calls to the child's process
	/// standard output and error streams.
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			WriteLine("Welcome to the colored WHERE command!", ConsoleColor.White);
			WriteLine("Enter the command arguments or press the Enter key to display help reference: ", ConsoleColor.White);
			var arguments = Console.ReadLine();
			arguments = string.IsNullOrWhiteSpace(arguments) ? "/?" : arguments;

			var whereProcess = new Process();
			whereProcess.StartInfo = new ProcessStartInfo
			{
				FileName = "where",
				UseShellExecute = false, // UseShellExecute should always set to false if streams redirection is used
				Arguments = arguments,
				RedirectStandardError = true,
				RedirectStandardOutput = true
			};
			whereProcess.ErrorDataReceived += ErrorDataReceivedHandler;
			whereProcess.OutputDataReceived += OutputDataReceivedHandler;

			whereProcess.Start();

			whereProcess.BeginOutputReadLine();
			whereProcess.BeginErrorReadLine();

			whereProcess.WaitForExit();
		}

		private static void OutputDataReceivedHandler(object sender, DataReceivedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(e.Data)) return;

			WriteLine(e.Data, ConsoleColor.Green);
		}

		private static void ErrorDataReceivedHandler(object sender, DataReceivedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(e.Data)) return;

			WriteLine(e.Data, ConsoleColor.Red);
		}

		private static void WriteLine(string line, ConsoleColor color)
		{
			var savedForegroundColor = Console.ForegroundColor;
			Console.ForegroundColor = color;

			Console.WriteLine(line);

			Console.ForegroundColor = savedForegroundColor;
		}
	}
}