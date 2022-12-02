using Attributes;

namespace TestApp;

public class Program
{
    public static void Main()
    {
        var cl = new ClassWithTest();
    }

    [Before]
    public void Before()
    {
        var a = 10000;
    }
    
    [Before]
    public void Before2(int x)
    {
        var a = 10000;
    }

    [After]
    public void After()
    {
        var b = 1 + 1;
    }
    
    [After]
    public void After2(string s)
    {
        var b = 1 + 1;
    }
    
    [After]
    public void After3()
    {
        throw new Exception();
    }


    [MyTest(Expected: false, Ignore: "i don't like this test")]
    public bool TestFun1()
    {
        return true;
    }

    [MyTest(Expected: true, Ignore: "idk")]
    public void TestFun2()
    {
        throw new Exception();
    }
}