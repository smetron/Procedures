/// <summary>
/// The RecurringTask class is used to execute a specified task at regular intervals.
/// It uses a Timer to trigger the task execution and ensures that the task is disposed of properly when the application exits.
/// </summary>
/// <example>
/// Usage example:
/// <code>
/// // Define the task to be executed
/// void MyTask()
/// {
///     Console.WriteLine("Task executed at: " + DateTime.Now);
/// }
///
/// // Create a RecurringTask instance to execute MyTask every minute
/// RecurringTask recurringTask = new RecurringTask(MyTask, TimeSpan.FromMinutes(1));
///
/// // Start the recurring task
/// recurringTask.Start();
/// </example>

using System;
using System.Timers;

namespace Lens
    public class RecurringTask : IDisposabl
      
      private readonly Action _task;
        private readonly double _intervalMilliseconds;
        private bool _disposed;

        public RecurringTask(Action task, TimeSpan interval)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _intervalMilliseconds = interval.TotalMilliseconds;
            _timer = new Timer(_intervalMilliseconds);
            _timer.Elapsed += (sender, e) => _task();
            _timer.AutoReset = true;

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        public void Start()
        {
            if (_timer != null)
            {
                _timer.Enabled = true;
                Console.WriteLine("Recurring task started.");
            }
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                Console.WriteLine("Recurring task stopped.");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer?.Stop();
                    _timer?.Dispose();
                    Console.WriteLine("Recurring task disposed.");
                }

                _disposed = true;
            }
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            Dispose();
        }

        ~RecurringTask()
        {
            Dispose(false);
        }
    }
}
