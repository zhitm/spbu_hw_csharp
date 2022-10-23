using System.Collections.Concurrent;
using Optional;
using ThreadPool.MyTask;

namespace ThreadPool;

public class MyThreadPool : IDisposable
{
    private int _nThreads;
    private bool _isShutdown;
    private List<Thread> _threads;
    private BlockingCollection<Action> _queue;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isDisposed;
    private readonly object _locker = new ();

    private void ThreadWork()
    {
        foreach (var action in _queue)
        {
            action();
        }
    }
    
    public MyThreadPool(int threadsCount)
    {
        _nThreads = threadsCount;
        _isShutdown = false;
        _threads = new List<Thread>(_nThreads);
        _queue = new BlockingCollection<Action>();
        _cancellationTokenSource = new CancellationTokenSource();
        
        for (var i = 0; i < _nThreads; i++)
        {
            _threads.Add(new Thread(() => ThreadWork()));
            _threads[i].Start();
        }
    }

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
                    _queue.Add(action);
                    newTask = new MyTask<TResult>(this, computation, _queue).Some<MyTask<TResult>>();
                }
                catch
                {
                }
            }
        }
        return newTask.ValueOr(()=>throw new Exception("task can't be created"));
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
    public void Shutdown()
    {
        lock (_locker)
        {
            if (!_isShutdown)
            {
                foreach (var threadItem in _threads)
                {
                    threadItem.Join();
                }
                _isShutdown = true;
            }
        }
    }
}