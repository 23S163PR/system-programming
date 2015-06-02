using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hybrid_sync_monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var transaction = new Transaction();
            Monitor.Enter(transaction);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.CurrentThread.IsBackground = false;
                transaction.ProcessTransaction();
            });

            Console.ReadLine();
            Monitor.Exit(transaction);
        }
    }

    public class Transaction : IDisposable
    {
        private ReaderWriterLockSlim @lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private DateTime lastTransactionTime;
        public DateTime LastTransactionTime
        {
            get
            {
                @lock.EnterReadLock();

                var l = lastTransactionTime; // just the code to show that we can have some " read" logic here

                @lock.ExitReadLock();

                return l; 
            }
        }

        public void ProcessTransaction()
        {
            @lock.EnterWriteLock();
            lastTransactionTime = DateTime.Now;
            @lock.ExitWriteLock();
        }

        public void Dispose()
        {
            @lock.Dispose();
        }
    }
}