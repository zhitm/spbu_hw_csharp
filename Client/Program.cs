using static System.Console;

using System.Net;
using System.Text;

namespace SimpleFTP;

public class Program
{
    static (IPAddress, int) ParseArgs(string[] args)
    {
        if (args.Length != 2)
        {
            throw new ArgumentException("Command line should contain as args only server ip and port");
        }

        if (!IPAddress.TryParse(args[0], out IPAddress? ip))
        {
            throw new ArgumentException("Command line should contain as args only server ip and port");

        }

        if (!int.TryParse(args[1], out int port))
        {
            throw new ArgumentException("Command line should contain as args only server ip and port");

        }

        return (ip, port);
    }
    public static void Main(string[] args)
    {
        
        var (ip, port) = ParseArgs(args);
        var client = new Client(ip, port);
     
        
        WriteLine("Format of input: ");
        WriteLine("list {path}");
        WriteLine("get {path}");
        WriteLine("you can input 'stop' to stop the client");
        WriteLine("Example: list .");
        
        while (true)
        {
            var str = ReadLine();
            if (str == "stop")
            {
                return;
            }

            if (str == null)
            {
                WriteLine("incorrect input");
                continue;
            }
            var strings = str.Split(' ');
            if (strings.Length != 2)
            {
                WriteLine("incorrect input");
                continue;
            }

            if (strings[0] == "list")
            {
                var answer = client.List(strings[1]).Result;
                var size = answer.Item1;
                WriteLine($"count of elements in dir: {size}");
                for (int i = 1; i <= size; i++)
                {
                    var element = answer.Item2[i - 1];
                    WriteLine($"{i}: {element.Item1} is dir: {element.Item2}");
                }
            }
            else if (strings[0] == "get")
            {
                var answer = client.Get(strings[1]).Result;
                WriteLine($"bytes count: {answer.Item1}");
                WriteLine($"content: {Encoding.Default.GetString(answer.Item2)}");
            }
            else
            {
                WriteLine("incorrect input");
                continue;
            }
            
        }
    }
}