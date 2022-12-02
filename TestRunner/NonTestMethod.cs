using System.Reflection;

namespace TestRunner;

public class NonTestMethod
{
    private readonly MethodInfo _methodInfo;
    private readonly object? _classInstance;
    private readonly string _methodName;
    public NonTestMethod(MethodInfo methodInfo, object? classInstance)
    {
        _methodInfo = methodInfo;
        _classInstance = classInstance;
        _methodName = methodInfo.Name;

    }

    public bool Invoke()
    {
        try
        {
            _methodInfo.Invoke(_classInstance, null);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Non-test FAIL: {_methodName}. Exception message: {e.Message}");
            return false;
        }
    }
}