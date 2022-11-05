using System;
using System.Dynamic;
using System.Net;

namespace SimpleFTP;

public class Program
{
    public static async Task Main()
    {
        var server = new Server(IPAddress.Parse("127.0.0.1"), 2390);
        // var cts = new CancellationToken();
        await server.StartListen();
        
    }
}