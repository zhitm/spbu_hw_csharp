using System.Reflection;

namespace TestRunner;

public class NonTestMethod
{
    private readonly MethodInfo _methodInfo;
    private readonly object? _classInstance;
    private readonly string _methodName;
    private readonly bool _isBefore;
    public TestMethodParams? TestParams;
    public NonTestMethod(MethodInfo methodInfo, object? classInstance, bool isBefore)
    {
        _methodInfo = methodInfo;
        _classInstance = classInstance;
        _methodName = methodInfo.Name;
        _isBefore = isBefore;
    }

    public bool Invoke()
    {
        try
        {
            _methodInfo.Invoke(_classInstance, null);
            TestParams = new TestMethodParams(_methodName, true, false, false, !_isBefore, _isBefore);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Non-test FAIL: {_methodName}. Exception message: {e.Message}");
            TestParams = new TestMethodParams(_methodName, false, false, false, !_isBefore, _isBefore);
            return false;
        }
    }
}