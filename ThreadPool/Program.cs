namespace ThreadPool;

class ThreadPoolMain
{
    public static int Foo()
    {
        Thread.Sleep(1000);
        Console.WriteLine("hi");
        return 1;
    }
    public static void Main()
    {
        var pool = new MyThreadPool(4);
        var task = pool.Submit(() => Foo());
        Console.WriteLine(task.Result);
        pool.Shutdown();
    }
}