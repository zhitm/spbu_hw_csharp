using System.Net;
using System.Net.Sockets;

namespace SimpleFTP;

public class Server
{
    private readonly IPAddress _address;
    private readonly int _port;

    public Server(IPAddress adress, int port)
    {
        _address = adress;
        _port = port;
    }

    private static async Task List(NetworkStream stream, string path)
    {
        using var streamWriter = new StreamWriter(stream);
        if (!Directory.Exists(path))
        {
            await streamWriter.WriteAsync("-1");
            await streamWriter.FlushAsync();
            return;
        }

        var directories = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);
        var size = directories.Length + files.Length;

        await streamWriter.WriteAsync(size.ToString());
        await streamWriter.FlushAsync();

        foreach (var file in files)
        {
            await streamWriter.WriteAsync($" {file} false");
            await streamWriter.FlushAsync();
        }

        foreach (var directory in directories)
        {
            await streamWriter.WriteAsync($" {directory} true");
            await streamWriter.FlushAsync();
        }
    }

    private static async Task Get(NetworkStream stream, string path)
    {
        using var streamWriter = new StreamWriter(stream);

        if (!File.Exists(path))
        {
            await streamWriter.WriteAsync("-1");
            await streamWriter.FlushAsync();
            return;
        }

        long size = (new FileInfo(path)).Length;

        await streamWriter.WriteLineAsync(size.ToString());
        await streamWriter.FlushAsync();

        using var fileStream = new FileStream(path, FileMode.Open);

        await fileStream.CopyToAsync(streamWriter.BaseStream);
        await streamWriter.FlushAsync();
    }

    public async Task StartListen(CancellationTokenSource cts)
    {
        var tcpListener = new TcpListener(_address, _port);
        tcpListener.Start();
        while (!cts.IsCancellationRequested)
        {
            using var socket = tcpListener.AcceptSocket();
            Console.WriteLine(socket.AddressFamily);
            await using var newtworkStream = new NetworkStream(socket);
            using var streamReader = new StreamReader(newtworkStream);
            var strings = (streamReader.ReadLine())?.Split(' ');
            
            if (strings == null || strings.Length != 2)
            {
                continue;
            }

            var msgType = strings[0];
            if (msgType == "list")
            {
                Console.WriteLine("list");
                await Task.Run(() => List(newtworkStream, strings[1]));
            }

            if (msgType == "get")
            {
                Console.WriteLine("get");
                await Task.Run(() => Get(newtworkStream, strings[1]));
            }
        }
        
        tcpListener.Stop();
    }
}