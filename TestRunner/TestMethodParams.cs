namespace TestRunner;

public class TestMethodParams
{
    public readonly string Name;
    public readonly bool IsPassed;
    public readonly bool IsIgnored;
    public readonly bool IsExcepted;
    public readonly bool IsAfter;
    public readonly bool IsBefore;

    public TestMethodParams(string name, bool isPassed, bool isIgnored, bool isExcepted, bool isAfter, bool isBefore)
    {
        this.Name = name;
        this.IsPassed = isPassed;
        this.IsIgnored = isIgnored;
        this.IsExcepted = isExcepted;
        this.IsAfter = isAfter;
        this.IsBefore = isBefore;
    }
}