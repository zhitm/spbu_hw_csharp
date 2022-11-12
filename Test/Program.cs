
namespace Test;

class Program
{
    public static void Main()
    {
        var queue = new MySafePriorityQueue<string, int>();
        var t = Task.Run(() => queue.Dequeue());
        Task.Run((() => queue.Enqueue("11", 10)));
        Console.WriteLine(t.Result);
        // queue.Enqueue("11", 10);
        // queue.Enqueue("1111", 0);
        // Console.WriteLine(queue.Dequeue());
    }
}