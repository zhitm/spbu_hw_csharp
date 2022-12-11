namespace TestTestRunner;

public class Tests
{
    TestRunner.TestRunner _runner = new();

    [SetUp]
    public void Setup()
    {
        _runner.ExecuteTests("../../../../TestApp/obj/Debug/net6.0/");
    }

    [Test]
    public void HasTested()
    {
        Assert.True(_runner.HasTested);
    }

    [Test]
    public void TestBeforeMethods()
    {
        var testingClasses = _runner.TestingClasses;
        var beforeMethods = testingClasses.SelectMany(testingClass => testingClass.BeforeMethods).ToList();
        var beforeParams = beforeMethods.Find(it => it.TestParams.Name == "Before").TestParams;
        var before2Params = beforeMethods.Find(it => it.TestParams.Name == "Before2").TestParams;
        Assert.Multiple(() =>
        {
            Assert.True(beforeParams.IsBefore);
            Assert.False(beforeParams.IsAfter);
            Assert.True(beforeParams.IsPassed);

            Assert.True(before2Params.IsBefore);
            Assert.False(before2Params.IsAfter);
            Assert.False(before2Params.IsPassed);
        });
    }

    [Test]
    public void TestAfterMethods()
    {
        var testingClasses = _runner.TestingClasses;
        var afterMethods = testingClasses.SelectMany(testingClass => testingClass.AfterMethods).ToList();
        var afterParams = afterMethods.Find(it => it.TestParams.Name == "After").TestParams;
        var after2Params = afterMethods.Find(it => it.TestParams.Name == "After2").TestParams;
        Assert.Multiple(() =>
        {
            Assert.False(afterParams.IsBefore);
            Assert.True(afterParams.IsAfter);
            Assert.True(afterParams.IsPassed);

            Assert.False(after2Params.IsBefore);
            Assert.True(after2Params.IsAfter);
            Assert.False(after2Params.IsPassed);
        });
    }

    [Test]
    public void TestTestMethods()
    {
        var testingClasses = _runner.TestingClasses;
        var testMethods = testingClasses.SelectMany(testingClass => testingClass.TestMethods).ToList();
        var testParams = testMethods.Find(it => it.TestParams.Name == "ICantDivideByZero").TestParams;
        var test2Params = testMethods.Find(it => it.TestParams.Name == "TestFun1").TestParams;
        Assert.Multiple(() =>
        {
            Assert.False(testParams.IsBefore);
            Assert.False(testParams.IsAfter);
            Assert.True(testParams.IsPassed);
            Assert.False(testParams.IsIgnored);
            Assert.True(testParams.IsExcepted);


            Assert.False(test2Params.IsBefore);
            Assert.False(test2Params.IsAfter);
            Assert.True(test2Params.IsPassed);
            Assert.True(test2Params.IsIgnored);
            Assert.False(test2Params.IsExcepted);
        });
    }
}