using System.Collections.Concurrent;

namespace ThreadPool.MyTask;

public class MyTask<TResult> : IMyTask<TResult>
{
    private readonly Computation<TResult> _computation;
    private readonly MyThreadPool _threadPool;
    public TResult Result { get {
        return GetThisTaskResult();
    } }
    private BlockingCollection<Action> _queue;

    public MyTask(MyThreadPool threadPool, Computation<TResult> computation, BlockingCollection<Action> queue)
    {
        _computation = computation;
        _threadPool = threadPool;
        _queue = queue;
    }

    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> continuation)
    {
        var newFunc = () =>
        {
            var result = GetThisTaskResult();
            return continuation.Invoke(result);
        };
        return _threadPool.Submit(newFunc);
    }

    private TResult GetThisTaskResult()
    {
        if (!_computation.IsComputed)
        {
            _computation.Compute();
        }

        return _computation.Result();
    }
}