using Attributes;

namespace TestApp;
using TestRunner;

public class ClassWithTest
{
    [MyTest(Expected: false)]
    public void TestNotFail()
    {
        var a = 1 + 1;
        a = 1 / 1;
    }
    
    
    [MyTest(Expected: true)]
    public void ICantDivideByZero()
    {
        throw new Exception("EEEEEEEEEEEERROR");
    }
}