using ThreadPool;

namespace TestThreadPool;

public class Tests
{
    [Test]
    public void TestContinueWith()
    {
        var pool = new MyThreadPool(4);

        var task1 = pool.Submit(() => 2 * 3);
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

        var task1 = pool.Submit(() => 2 * 3);
        var task2 = pool.Submit(() => 2 * 4);

        Assert.Multiple(() =>
        {
            Assert.AreEqual(task1.Result, 6);
            Assert.AreEqual(task2.Result, 8);
        });
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

    [Test]
    public void TestRnd()
    {
        var pool = new MyThreadPool(10);
        Random rnd = new Random();
        var task = pool.Submit(() => rnd.Next(1, 1000000));
        var result1 = task.Result;
        var result2 = task.Result;
        Assert.AreEqual(result1, result2);
        pool.Shutdown();
    }

    [Test]
    public void TestShutdown()
    {
        var pool = new MyThreadPool(10);
        var task1 = pool.Submit(() => 2 * 3);
        pool.Shutdown();
        Assert.Throws<AggregateException>(() => pool.Submit(() => 2 * 3));
    }


    [Test]
    public void TestManyTasks()
    {
        var pool = new MyThreadPool(10);

        var listOfTasks = new List<MyThreadPool.MyTask<int>>();
        for (int i = 0; i < 99; i++)
        {
            var index = i;
            listOfTasks.Add(pool.Submit(() => index));
        }

        Assert.Multiple(() =>
        {
            for (int i = 0; i < 99; i++)
            {
                var index = i;
                listOfTasks.Add(pool.Submit((() => index)));
                Assert.AreEqual(i, listOfTasks[i].Result);
            }
        });
        pool.Shutdown();
    }
}