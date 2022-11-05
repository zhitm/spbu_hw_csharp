using System.Net;
using System.Net.Sockets;

namespace SimpleFTP;

public class Client
{
    private readonly IPAddress _address;
    private readonly int _port;
    
    private readonly IPAddress _ServerAddress;
    private readonly int _ServerPort;

    public Client(IPAddress address, int port, IPAddress serverAddress, int serverPort)
    {
        _address = address;
        _port = port;
        _ServerAddress = serverAddress;
        _ServerPort = serverPort;
    }
    
    
     public async Task<(int, List<(string, bool)>)> List(string pathToDirectory)
    {
        var client = new TcpClient();
        await client.ConnectAsync(_ServerAddress, _ServerPort);
        
        using var stream = client.GetStream();
        using var streamWriter = new StreamWriter(stream);
        
        await streamWriter.WriteLineAsync($"list {pathToDirectory}");
        await streamWriter.FlushAsync();
        
        using var streamReader = new StreamReader(stream);
        var data = await streamReader.ReadLineAsync();
        if (data == null)
        {
            throw new InvalidDataException();
        }

        var strings = data.Split(' ');
        if (!int.TryParse(strings[0], out int size))
        {
            throw new InvalidDataException();
        }

        if (size == -1)
        {
            throw new DirectoryNotFoundException();
        }

        var dirContentWithFlags = new List<(string, bool)>();
        for (int i = 1; i < strings.Length; i++)
        {
            bool flag = (strings[i + 1] == "true");
            dirContentWithFlags.Add((strings[i], flag));
            i++;
        }
        return (size, dirContentWithFlags);
    }
     
    public async Task<(int, byte[])> Get(string pathToFile)
    {
        var client = new TcpClient();
        await client.ConnectAsync(_address, _port);
        using var stream = client.GetStream();
        using var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteLineAsync($"get {pathToFile}");
        await streamWriter.FlushAsync();
        using var streamReader = new StreamReader(stream);
        var stringWithSize = (await streamReader.ReadLineAsync());
        if (!int.TryParse(stringWithSize, out int size))
        {
            throw new InvalidDataException();
        }

        if (size == -1)
        {
            throw new FileNotFoundException();
        }

        var buffer = new byte[size];
        await streamReader.BaseStream.ReadAsync(buffer, 0, size);
        return (size, buffer);
    }
}