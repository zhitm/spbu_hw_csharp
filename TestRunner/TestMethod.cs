using System.Reflection;

namespace TestRunner;

public class TestMethod
{
    private readonly MethodInfo _methodInfo;
    private readonly object? _classInstance;
    private readonly bool _isExcepted;
    private readonly bool _isIgnored;
    private readonly string? _ignore;
    private readonly string _testName;

    public TestMethod(MethodInfo methodInfo, object? classInstance, bool isExcepted, bool isIgnored, string? ignore)
    {
        _methodInfo = methodInfo;
        _classInstance = classInstance;
        _isExcepted = isExcepted;
        _isIgnored = isIgnored;
        _ignore = ignore;
        _testName = methodInfo.Name;
    }

    public object? Invoke()
    {
        if (!_isIgnored)
        {
            if (_isExcepted)
            {
                
            }
            return _methodInfo.Invoke(_classInstance, null);
        }
        Console.WriteLine($"Test {_testName} is ignored: {_ignore}");
        return null;
    }
}