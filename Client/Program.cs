using System;
using System.Dynamic;
using System.Net;

namespace Client;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(1);

        var client = new SimpleFTP.Client(IPAddress.Parse("127.0.0.1"), 2290, IPAddress.Parse("127.0.0.1"), 2390);
        var ans = client.List(".");
        Console.WriteLine(ans.Result.Item1);
        Console.WriteLine(ans.Result.Item2[0]);
    }
}