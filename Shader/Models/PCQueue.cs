using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Models
{
    class PCQueue : IDisposable 
    {
        readonly object _locker = new object();
        Thread[] _workers;
        Queue<Action> _itemQ = new Queue<Action>();

        public PCQueue(int workerCount)
        {
            _workers = new Thread[workerCount];

            // Create and start a separate thread for each worker
            for (int i = 0; i < workerCount; i++)
                (_workers[i] = new Thread(Consume) { IsBackground = true }).Start();
        }

        public void Dispose()
        {
            // Enqueue one null task per worker to make each exit.
            foreach (Thread worker in _workers) EnqueueItem(null);
            foreach (Thread worker in _workers) worker.Join();
        }

        public void EnqueueItem(Action actIn)
        {
            lock (_locker)
            {
                _itemQ.Enqueue(actIn);           // We must pulse because we're
                Monitor.Pulse(_locker);         // changing a blocking condition.
            }
        }

        void Consume()
        {
            while (true)                        // Keep consuming until
            {                                   // told otherwise.
                Action item;
                lock (_locker)
                {
                    while (_itemQ.Count == 0) Monitor.Wait(_locker);
                    item = _itemQ.Dequeue();
                }
                if (item == null) return;         // This signals our exit.
                item();                           // Execute item.
            }
        }
    }
}
