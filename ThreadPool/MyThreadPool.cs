using System.Collections.Concurrent;
using Optional;
using ThreadPool.MyTask;

namespace ThreadPool;

public class MyThreadPool : IDisposable
{
    private bool _isShutdown;
    private readonly List<Thread> _threads;
    private readonly BlockingCollection<Action> _queue = new();
    private CancellationTokenSource _cancellationTokenSource = new();
    private bool _isDisposed = false;
    private readonly object _locker = new();

    private void ThreadWork()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            lock (_locker)
            {
                while (_queue.Count == 0 && !_cancellationTokenSource.IsCancellationRequested)
                {
                    //is waiting for adding element for queue or shutdown
                    Monitor.Wait(_locker);
                }

                if (_queue.TryTake(out Action action))
                {
                    action();
                }
            }
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <typeparam name="threadsCount">Count of working threads</typeparam>
    public MyThreadPool(int threadsCount)
    {
        _isShutdown = false;
        _threads = new List<Thread>(threadsCount);

        for (var i = 0; i < threadsCount; i++)
        {
            _threads.Add(new Thread(ThreadWork));
            _threads[i].Start();
        }
    }

    private void AddElementToQueue(Action action)
    {
        _queue.Add(action);
        Monitor.Pulse(_locker);
    }

    /// <summary>
    /// Method for returning a task to be accepted for execution.
    /// </summary>
    /// <typeparam name="TResult">Type of return value</typeparam>
    /// <param name="func">Method</param>
    public MyTask<TResult> Submit<TResult>(Func<TResult> func)
    {
        var newTask = Option.None<MyTask<TResult>>();

        lock (_locker)
        {
            if (!_isDisposed && !_isShutdown)
            {
                try
                {
                    var computation = new Computation<TResult>(func);
                    var action = () => computation.Compute();
                    AddElementToQueue(action);
                    newTask = new MyTask<TResult>(this, computation).Some();
                }
                catch
                {
                    return newTask.ValueOr(() => throw new AggregateException("submit error"));
                }
            }
        }

        return newTask.ValueOr(() => throw new AggregateException("task can't be created"));
    }


    public void Dispose()
    {
        if (!_isShutdown)
        {
            Shutdown();
        }

        lock (_locker)
        {
            if (!_isDisposed)
            {
                _queue.Dispose();
            }

            _isDisposed = true;
        }
    }

    /// <summary>
    /// Method for shutting down threads
    /// </summary>
    public void Shutdown()
    {
        lock (_locker)
        {
            if (!_isShutdown)
            {
                _cancellationTokenSource.Cancel();
                _queue.CompleteAdding();
                _isShutdown = true;
                //pulse all waiting threads so all threads will end work
                Monitor.PulseAll(_locker);
            }
        }
    }
}