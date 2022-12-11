using System.Reflection;

namespace TestRunner;

public class TestingClass
{
    public List<NonTestMethod> BeforeMethods = new();
    public readonly List<TestMethod> TestMethods = new();
    public List<NonTestMethod> AfterMethods = new();
    private readonly string _className;
    private volatile int _failedTests;
    private volatile int _failedBefore;
    private volatile int _failedAfter;

    public TestingClass(MemberInfo classType)
    {
        _className = classType.Name;
    }

    public void RunTests()
    {
        Console.WriteLine("--------------------------------------");
        Console.WriteLine($"Started testing of {_className} class");
        Console.WriteLine($"{_className} class before-testing methods are running...");

        Parallel.ForEach(BeforeMethods, method =>
        {
            if (!method.Invoke()) Interlocked.Increment(ref _failedBefore);
        });

        Console.WriteLine($"{_className} class test methods are running...");
        Parallel.ForEach(TestMethods, test =>
        {
            if (!test.Invoke()) Interlocked.Increment(ref _failedTests);
        });

        Console.WriteLine($"{_className} class after-testing methods are running...");
        Parallel.ForEach(AfterMethods, method =>
        {
            if (!method.Invoke()) Interlocked.Increment(ref _failedAfter);
        });


        Console.WriteLine(
            $"{_className} tested. Failed before: {_failedBefore}. Failed tests: {_failedTests}. Failed after: {_failedAfter}");
    }
}