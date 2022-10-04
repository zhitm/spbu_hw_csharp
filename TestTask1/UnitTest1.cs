using Task1;


namespace TestTask1;

[TestFixture]
public class Tests
{
    [Test]
    [TestCase(new int[] { 9, 2, 1, 3, 4 })]
    [TestCase(new int[] { 1 })]
    [TestCase(new int[] { })]
    [TestCase(new int[] { 10000, 2, 4, 1 })]
    [TestCase(new int[] { 1, 3, 5, 23, 53, 46, 345, 235, 23, 52, 5, 46, 3456, 3, 63 })]
    public void TestBubbleSort(int[] arr)
    {
        Program.BubbleSort(arr);
        Assert.True(Program.IsSorted(arr));
    }

    [Test]
    [TestCase(new int[] { 9, 2, 1, 3, 4 }, false)]
    [TestCase(new int[] { 1, 2, 3 }, true)]
    [TestCase(new int[] { 1 }, true)]
    [TestCase(new int[] { }, true)]
    public void TestIsSorted(int[] arr, bool isSorted)
    {
        Assert.AreEqual(Program.IsSorted(arr), isSorted);
    }
}