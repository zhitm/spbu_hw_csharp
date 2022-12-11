
namespace TestRunner;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Please enter a path to dll as argument.");
            return;
        }

        var path = args[0];
        var tester = new TestRunner();
        if (Directory.Exists(path))
        {
            tester.ExecuteTests(path);
        }
        else
        {
            Console.WriteLine("It's not a path");
        }
    }
}