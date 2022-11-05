using System.Net;
using static System.Console;

namespace SimpleFTP;

public class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length != 2)
        {
            WriteLine("Command line should contain as args only server ip and port");
            return;
        }

        if (!IPAddress.TryParse(args[0], out IPAddress? ip))
        {
            WriteLine("Incorrect ip");
            return;
        }

        if (!int.TryParse(args[1], out int port))
        {
            WriteLine("Incorrect ip");
            return;
        }
        var server = new Server(ip, port);
        var cts = new CancellationTokenSource();
        await server.StartListen(cts);
    }
}