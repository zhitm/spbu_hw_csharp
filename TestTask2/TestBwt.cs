using Task2;

namespace TestTask2;
[TestFixture]
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestBwt()
    {
        Assert.AreEqual(Program.Bwt("ABACABA"), ("BCABAAA", 2));
    }

    [Test]
    public void TestReverseBwt()
    {
        Assert.AreEqual(Program.ReverseBwt("BCABAAA", 2), "ABACABA");
    }

    [Test]
    [TestCase("abcdsdfgdfhgdfgSgdf")]
    [TestCase("bdfghdhdfg")]
    [TestCase("agdfhdgjfggsetwe")]
    [TestCase("afghfhghfghdfghdrgwr")]
    [TestCase("aergergerghegergerghhthd")]
    public void TestComposition(string str)
    {
        var optimized = Program.Bwt(str);
        Assert.AreEqual(Program.ReverseBwt(optimized.Item1, optimized.Item2), str);
    }
}