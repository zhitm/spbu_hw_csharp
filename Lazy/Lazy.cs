namespace Lazy;

public class Lazy<T> : ILazy<T>
{
    private T? _calculatedValue;
    private readonly Func<T?> _func;
    public bool IsValueCalculated { get; private set; }

    public Lazy(Func<T?> func)
    {
        _func = func;
    }

    public T? Get()
    {
        if (IsValueCalculated) return _calculatedValue;
        _calculatedValue = _func();
        IsValueCalculated = true;
        return _calculatedValue;
    }
}