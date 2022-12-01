using System.Reflection;

namespace TestRunner;

public class TestRunner
{
    
    private readonly List<TestMethod> _methods = new List<TestMethod>();

    private void FindTests(string path)
    {
        Assembly assembly =
            Assembly.LoadFrom(path);
        foreach (var type in assembly.ExportedTypes)
        {
            if (!type.IsClass) continue;
            var tests = FindTestsInClass(type);
            var instance = Activator.CreateInstance(type);
            foreach (var test in tests)
            {
                var attr = GetTestAttribute(test);
                // Console.WriteLine(attr);
                // _methods.Add(new TestMethod(test, instance));
            }
        }
    }

    public void ExecuteTests(string path)
    {
        FindTests(path);
        foreach (var method in _methods)
        {
            Console.WriteLine(method.Invoke());
        }
    }

    private static Attribute GetTestAttribute(MethodInfo methodInfo)
    {
        foreach (var data in methodInfo.GetCustomAttributesData())
        {
            Console.WriteLine(">>>>>>>>>>>>>");
            foreach (var argument in data.NamedArguments)
            {
                Console.WriteLine(argument.MemberInfo);
            }
            Console.WriteLine(methodInfo.Name);
        }
        foreach (var attr in methodInfo.GetCustomAttributes())
        {
            if (attr.GetType().ToString() == typeof(MyTest).ToString()) return attr;
        }

        throw new Exception("This method hasn't this attr");
    }
    
    private static bool HasTestAttr(MethodInfo methodInfo)
    {
        return methodInfo.GetCustomAttributes()
            .Any(attr => attr.GetType().ToString() == typeof(MyTest).ToString());
    }

    private static bool IsTestIgnored(MethodInfo method) => false;

    private static List<MethodInfo> FindTestsInClass(Type classType)
    {
        var tests = classType.GetMethods()
            .Where(HasTestAttr)
            .ToList();
        return tests;
    }
}