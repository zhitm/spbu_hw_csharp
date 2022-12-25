namespace ThreadPool.MyTask;

public class MyTask<TResult> : IMyTask<TResult>
{
    private readonly Computation<TResult> _computation;
    private readonly MyThreadPool _threadPool;
    public TResult Result => GetThisTaskResult();

    public MyTask(MyThreadPool threadPool, Computation<TResult> computation)
    {
        _computation = computation;
        _threadPool = threadPool;
    }

    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> continuation)
    {
        TNewResult NewFunc()
        {
            var result = GetThisTaskResult();
            return continuation.Invoke(result);
        }

        return _threadPool.Submit(NewFunc);
    }

    private TResult GetThisTaskResult()
    {
        lock (_computation.Locker)
        {
            if (!_computation.IsComputed)
            {
                Monitor.Wait(_computation.Locker);
                // _computation.Compute();
            }

            if (!_computation.IsException) return _computation.Result().Invoke();
            throw new AggregateException(_computation.ExceptionMsg);
        }
    }
}