namespace TestRunner;

[AttributeUsage(AttributeTargets.Method)]
public class MyTest : Attribute
{
    public bool expected;

    public string? ignore = null;


    public MyTest(bool Expected = false, string Ignore = "")
    {
        expected = Expected;
        ignore = Ignore;
    }
}

public class Before : Attribute
{
}

public class After : Attribute
{
}