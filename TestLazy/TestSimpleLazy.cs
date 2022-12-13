using Lazy;

namespace TestLazy;

public class TestsSimpleLazy
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
    public void TestRandom()
    {
        var random = new Random();
        var lazy = LazyFactory.CreateSimpleLazy(() => random.Next(12345));
        Assert.False(lazy.IsValueCalculated);
        var first = lazy.Get();
        var second = lazy.Get();
        Assert.AreEqual(lazy.Get(), first);
        Assert.AreEqual(lazy.Get(), first);
        Assert.True(lazy.IsValueCalculated);
    }
}