

using TestRunner;

namespace TestApp;

public class Program
{
    public static void Main()
    {
        var cl = new ClassWithTest();
    }

    [MyTest(Expected: false, Ignore: "i don't like this test")]
    public bool testFun1()
    {
        return true;
    }
    
    [MyTest(Ignore:"fgsrgdgdr")]
    public int testFun2()
    {
        return 10;
    }
}