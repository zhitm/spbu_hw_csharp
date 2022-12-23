using ThreadPool;

namespace TestThreadPool;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestContinueWith()
    {
        var pool = new MyThreadPool(4);
        
        var task1 = pool.Submit(() => 2*3);
        var task2 = task1.ContinueWith((x => 2 * x));

        Assert.Multiple((() =>
        {
            Assert.AreEqual(task1.Result, 6);
            Assert.AreEqual(task2.Result, 12);
        }));
        pool.Shutdown();
    }
    
    
    [Test]
    public void TestCalculation()
    {
        var pool = new MyThreadPool(4);
        
        var task1 = pool.Submit(() => 2*3);
        var task2 = pool.Submit(() => 2*4);

        Assert.Multiple((() =>
        {
            Assert.AreEqual(task1.Result, 6);
            Assert.AreEqual(task2.Result, 8);
        }));
        pool.Shutdown();
    }
    
    
    [Test]
    public void ShouldAggregateExceptionWhenDivideByZero()
    {
        var pool = new MyThreadPool(10);
        int zero = 0;
        var task = pool.Submit(() => 1 / zero);
        int ReturnResult() => task.Result;
        Assert.Throws<AggregateException>(() => ReturnResult());
        pool.Shutdown();
    }
}