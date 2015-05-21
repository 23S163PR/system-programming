using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace task_factory
{
	class Program
	{
		static void Main(string[] args)
		{
			var parent = new Task(() =>
			{
				var cts = new CancellationTokenSource();
				var tf = new TaskFactory(
					cts.Token,
					TaskCreationOptions.AttachedToParent,
					TaskContinuationOptions.ExecuteSynchronously,
					TaskScheduler.Default);

				var childTasks = new[]
				{
					tf.StartNew(() => Sum(cts.Token, 10000)),
					tf.StartNew(() => Sum(cts.Token, 20000)),
					tf.StartNew(() => Sum(cts.Token, int.MaxValue)) // This should throw OverflowException
				};

				for (int task = 0; task < childTasks.Length; task++)
				{
					childTasks[task].ContinueWith(t => cts.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
				}

				tf
					.ContinueWhenAll(
						childTasks,
						completedTasks => completedTasks.Where(t => !t.IsFaulted && !t.IsCanceled).Max(t => t.Result),
						CancellationToken.None)
					.ContinueWith(t => Console.WriteLine("The maximum is: {0}", t.Result), TaskContinuationOptions.ExecuteSynchronously);
			});

			parent.ContinueWith(p => {
				var sb = new StringBuilder("The following exceptions occured:\n");

				foreach (var e in p.Exception.Flatten().InnerExceptions)
				{
					sb.AppendFormat("\t{0}\n", e.GetType().ToString());
				}

				Console.WriteLine(sb.ToString());
			}, TaskContinuationOptions.OnlyOnFaulted);

			parent.Start();

			Console.ReadKey();
		}

		private static int Sum(CancellationToken cancellationToken, int count)
		{
			throw new NotImplementedException();
		}
	}
}
