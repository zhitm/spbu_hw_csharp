// See https://aka.ms/new-console-template for more information
namespace TestRunner;
public class Program
{
    public static void Main()
    {
        var tester = new TestRunner();
        tester.ExecuteTests("C:/Users/Maria/RiderProjects/spbu_hw_csharp/TestApp/obj/Debug/net6.0/TestApp.dll");
    }

   
}
