using Lazy;

namespace TestLazy;

public class TestsThreadSafeLazy
{
    [Test]
    public void TestCalc()
    {
        var lazy = LazyFactory.CreateSimpleLazy(() => 2 + 3);
        Assert.False(lazy.IsValueCalculated);
        Assert.AreEqual(5, lazy.Get());
        Assert.True(lazy.IsValueCalculated);
    }

    [Test]
    public void TestNullFunc()
    {
        var lazy = LazyFactory.CreateSimpleLazy<int?>(() => null);
        Assert.False(lazy.IsValueCalculated);
        Assert.AreEqual(null, lazy.Get());
        Assert.True(lazy.IsValueCalculated);
    }


    [Test]
    public void TestRace()
    {
        var lazy = LazyFactory.CreateThreadSafeLazy(() => Thread.CurrentThread.ManagedThreadId);
        var computerId = (0, 0);
        var thread1 = new Thread(() =>
        {
            var compution = lazy.Get();
            computerId.Item1 = compution;
        });
        var thread2 = new Thread(() =>
        {
            var compution = lazy.Get();

            computerId.Item2 = compution;
        });
        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();
        Assert.AreEqual(computerId.Item1, computerId.Item2);
        Assert.True(computerId.Item1 != 0 && computerId.Item2 != 0);
    }
}