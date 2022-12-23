namespace ThreadPool;

class ThreadPoolMain
{
    public static void Main()
    {
        var pool = new MyThreadPool(4);
        var task = pool.Submit(() => 1 / 1);
        Console.WriteLine(task.Result);
        pool.Shutdown();
    }
}