namespace Lazy;

public class ThreadSafeLazy<T> : ILazy<T>
{
    private T? _calculatedValue;
    private readonly object _locker = new();
    private volatile bool _isValueCalculated;
    private readonly Func<T> _func;

    public ThreadSafeLazy(Func<T> func)
    {
        _func = func;
    }

    public T? Get()
    {
        if (_isValueCalculated) return _calculatedValue;
        lock (_locker)
        {
            if (_isValueCalculated) return _calculatedValue;
            _calculatedValue = _func();
            _isValueCalculated = true;
            return _calculatedValue;
        }
    }
}