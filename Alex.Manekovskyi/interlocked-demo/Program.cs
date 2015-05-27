using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace interlocked_demo
{
	class Program
	{
		static void Main(string[] args)
		{
			var requests = new MultiWebRequests(5);
			Console.ReadKey();
		}
	}

	internal class MultiWebRequests
	{
		private AsyncCoordinator coordinator = new AsyncCoordinator();
		private Dictionary<string, object> servers = new Dictionary<string, object> {
			{ "http://google.com", null },
			{ "http://microsoft.com", null },
			{ "http://itstep.org", null }
		};

		public MultiWebRequests(int timeout = Timeout.Infinite)
		{
			var httpClient = new HttpClient();
			foreach (var server in servers.Keys)
			{
				coordinator.AboutToStart(1 /* number of planned requests */);
                httpClient.GetByteArrayAsync(server)
					.ContinueWith(task => ComputeResult(server, task));
			}

			coordinator.AllStarted(AllFinished, timeout);
        }

		public void Cancel()
		{
			coordinator.Cancel();
		}

		private void ComputeResult(string server, Task<byte[]> task)
		{
			object result = null;
			if (task.Exception != null)
			{
				result = task.Exception;
			}
			else
			{
				result = task.Result.Length;
			}

			servers[server] = result;

			coordinator.OperationFinished();
		}

		private void AllFinished(CoordinationStatus status)
		{
			switch (status)
			{
				case CoordinationStatus.Cancelled:
					Console.WriteLine("Operation canceled.");
					break;
				case CoordinationStatus.Timeout:
					Console.WriteLine("Operation timeout.");
					break;
				case CoordinationStatus.Completed:
					Console.WriteLine("Operation completed. Results:");
					foreach (var kvp in servers)
					{
						var server = kvp.Key;
						var result = kvp.Value;
						Console.Write("* Server \"{0}\": ", server);
						if (result is Exception)
						{
							Console.WriteLine("failed due to {0}.", result.GetType().Name);
						}
						else
						{
							Console.WriteLine("returned {0:N0} bytes.", result);
						}
					}
					break;
			}
		}
	}

	internal enum CoordinationStatus
	{
		Cancelled,
		Timeout,
		Completed
	}

	internal class AsyncCoordinator
	{
		private int statusReported = 0;
        private int operationsCount = 0;
        private Action<CoordinationStatus> allFinishedCallback;
		private Timer timer;

		public void AboutToStart(int asyncOperationsCount)
		{
			Interlocked.Add(ref operationsCount, asyncOperationsCount);
		}

		public void OperationFinished()
		{
			if (Interlocked.Decrement(ref operationsCount) == 0)
			{
				ReportStatus(CoordinationStatus.Completed);
			}
		}

		public void AllStarted(Action<CoordinationStatus> allFinished, int timeout = Timeout.Infinite)
		{
			allFinishedCallback = allFinished;
			if (timeout != Timeout.Infinite)
			{
				timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
			}
		}

		public void Cancel()
		{
			ReportStatus(CoordinationStatus.Cancelled);
		}

		private void TimeExpired(object state)
		{
			ReportStatus(CoordinationStatus.Timeout);
		}

		private void ReportStatus(CoordinationStatus status)
		{
			if (Interlocked.Exchange(ref statusReported, 1) == 0)
			{
				allFinishedCallback(status);
			}
		}
    }
}
