namespace Attributes;

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

[AttributeUsage(AttributeTargets.Method)]
public class Before : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public class After : Attribute
{
}