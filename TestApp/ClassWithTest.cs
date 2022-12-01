namespace TestApp;
using TestRunner;

public class ClassWithTest
{
    [MyTest(Expected: false)]
    public string Test()
    {
        return "a" + "a";
    }
}