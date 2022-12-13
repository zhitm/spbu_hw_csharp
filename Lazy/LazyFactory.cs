namespace Lazy;

public class LazyFactory
{
    public static Lazy<T> CreateSimpleLazy<T>(Func<T?> func)
    {
        return new Lazy<T>(func);
    }

    public static ThreadSafeLazy<T> CreateThreadSafeLazy<T>(Func<T?> func)
    {
        return new ThreadSafeLazy<T>(func);
    }
}