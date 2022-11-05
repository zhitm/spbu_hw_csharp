using System.Net;
using SimpleFTP;

namespace TestFTP;

public class Tests
{
    int port = 2345;
    IPAddress ip = IPAddress.Parse("127.0.0.1");

    [SetUp]
    public void Setup()
    {
        Server server = new Server(ip, port);
        var cts = new CancellationTokenSource();
        var serverTask = Task.Run(() => server.StartListen(cts), cts.Token);
    }

    [Test]
    public async Task TestEmptyFileTransfer()
    {
        var client = new Client(ip, port);
        var answer = await client.Get("../../../TestFiles/empty.txt");
        Assert.That(answer, Is.EqualTo((0, new Byte[0])));
    }

    [Test]
    public void TestNonexistentDirectoryList()
    {
        var client = new Client(ip, port);
        Assert.ThrowsAsync<DirectoryNotFoundException>(() => client.List("../../../TestFiles/WHAT!.txt"));
    }

    [Test]
    public void TestNonexistentFileGet()
    {
        var client = new Client(ip, port);
        Assert.ThrowsAsync<FileNotFoundException>(() => client.Get("../../../TestFiles/WHAT!.txt"));
    }

    [Test]
    public async Task TestParallel()
    {
        var client1 = new Client(ip, port);
        var client2 = new Client(ip, port);
        var ans1 = await client1.Get("../../../TestFiles/big.txt");
        var ans2 = await client2.Get("../../../TestFiles/big.txt");
        Assert.Multiple(() =>
        {
            Assert.That(ans1.Item1, Is.EqualTo(ans2.Item1));
            Assert.That(ans1.Item1, !Is.EqualTo(0));
            Assert.That(ans1.Item2, Is.EquivalentTo(ans2.Item2));
        });
    }
}