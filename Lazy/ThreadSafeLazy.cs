namespace Lazy;

public class ThreadSafeLazy<T> : ILazy<T>
{
    private T? _calculatedValue;
    private readonly object _locker = new();
    public bool IsValueCalculated { get; private set; }
    private readonly Func<T> _func;

    public ThreadSafeLazy(Func<T> func)
    {
        _func = func;
    }

    public T? Get()
    {
        if (IsValueCalculated) return _calculatedValue;
        lock (_locker)
        {
            if (IsValueCalculated) return _calculatedValue;
            _calculatedValue = _func();
            IsValueCalculated = true;
            return _calculatedValue;
        }
    }
}