using Optional;

namespace ThreadPool;

public class Computation<TResult>
{
    private readonly Func<TResult> _func;
    private readonly object _locker = new();

    private Option<Func<TResult>> _result = Option.None<Func<TResult>>();

    // private readonly Action _action;
    private bool _funcIsComputed;

    public bool IsComputed
    {
        get { return _funcIsComputed; }

    }

    public TResult Result()
    {
        if (_result.HasValue) return _func.Invoke();
        Compute();
        return _func.Invoke();
    }

    public Computation(Func<TResult> func)
    {
        _func = func;
        // _action = action;
    }

    public void Compute()
    {
        lock (_locker)
        {
            if (!_funcIsComputed)
            {
                try
                {
                    var newResultFunc = () => _func.Invoke();
                    _result = (newResultFunc).Some();
                }
                catch (Exception exceptionResult)
                {
                    var newExceptionFunc = new Func<TResult>(() => throw exceptionResult);
                    _result = newExceptionFunc.Some();
                }
                finally
                {
                    _funcIsComputed = true;
                }
            }
        }
    }
}