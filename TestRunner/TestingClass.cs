using System.Reflection;

namespace TestRunner;

public class TestingClass
{
    public List<NonTestMethod> BeforeMethods = new();
    public readonly List<TestMethod> TestMethods = new();
    public List<NonTestMethod> AfterMethods = new();
    private readonly string _className;
    private int failedTests = 0;
    private int failedBefore = 0;
    private int failedAfter = 0;

    public TestingClass(MemberInfo classType)
    {
        _className = classType.Name;
    }
    public void RunTests()
    {
        Console.WriteLine("--------------------------------------");
        Console.WriteLine($"Started testing of {_className} class");
        Console.WriteLine($"{_className} class before-testing methods are running...");

        foreach (var beforeMethod in BeforeMethods)
        {
            if (!beforeMethod.Invoke()) failedBefore++;
        }
        Console.WriteLine($"{_className} class test methods are running...");

        foreach (var testMethod in TestMethods)
        {
            if (!testMethod.Invoke()) failedTests++;
        }
        Console.WriteLine($"{_className} class after-testing methods are running...");

        foreach (var afterMethod in AfterMethods)
        {
            if (!afterMethod.Invoke()) failedAfter++;
        }

        Console.WriteLine(
            $"{_className} tested. Failed before: {failedBefore}. Failed tests: {failedTests}. Failed after: {failedAfter}");   
    }
}