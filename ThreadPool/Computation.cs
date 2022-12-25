using Optional;
using Optional.Unsafe;

namespace ThreadPool;

public class Computation<TResult>
{
    private readonly Func<TResult> _func;
    public readonly object Locker = new();

    private Option<Func<TResult>> _result = Option.None<Func<TResult>>();

    private bool _funcIsComputed;

    public bool IsException;
    public string ExceptionMsg;
    public bool IsComputed => _funcIsComputed;


    public Func<TResult> Result()
    {
        // Compute();
        return _result.ValueOrDefault();
    }

    public Computation(Func<TResult> func)
    {
        _func = func;
    }

    /// <summary>
    /// Return method returns result of function invoke
    /// </summary>
    public void Compute()
    {
        lock (Locker)
        {
            if (!_funcIsComputed)
            {
                try
                {
                    TResult result = _func.Invoke();
                    TResult NewResultFunc() => result;
                    NewResultFunc();
                    _result = ((Func<TResult>?)NewResultFunc).Some();
                }
                catch (Exception exceptionResult)
                {
                    IsException = true;
                    ExceptionMsg = exceptionResult.Message;
                    var newExceptionFunc =
                        new Func<TResult>(() => throw new AggregateException(exceptionResult.Message));
                    _result = newExceptionFunc.Some();
                }
                finally
                {
                    _funcIsComputed = true;
                    Monitor.Pulse(Locker);
                }
            }
        }
    }
}