using System;
using System.Threading;

namespace recursive_autoresetevent
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    internal class RecursiveAutoResetEvent: IDisposable
    {
        private AutoResetEvent autoResetEvent = new AutoResetEvent(initialState: true);
        private int ownerThreadId = 0;
        private int counter = 0;

        public void Enter()
        {
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (ownerThreadId == currentThreadId)
            {
                counter++;
                return;
            }

            autoResetEvent.WaitOne();

            ownerThreadId = currentThreadId;
            counter = 1;
        }

        public void Leave()
        {
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (ownerThreadId != currentThreadId)
            {
                throw new InvalidOperationException();
            }

            if (--counter == 0)
            {
                ownerThreadId = 0;
                autoResetEvent.Set();
            }
        }

        public void Dispose()
        {
            autoResetEvent.Dispose();
        }
    }
}
