using System.Reflection;
using Attributes;

namespace TestRunner;

public class TestRunner
{
    public bool HasTested;
    public readonly List<TestingClass> TestingClasses = new();

    private void LoadTests(string path)
    {
        var types = Directory.EnumerateFiles(path, "*.dll", 0).Select(Assembly.LoadFrom)
            .SelectMany(assembly => assembly.ExportedTypes);
        if (!types.Any())
        {
            Console.WriteLine("Nothing to test");
        }

        foreach (var type in types)
        {
            if (!type.IsClass) continue;
            var newTestingClass = new TestingClass(type);
            var instance = Activator.CreateInstance(type);

            TestingClasses.Add(newTestingClass);
            newTestingClass.BeforeMethods = FindMethodsByAttrInClass(type, typeof(Before))
                .ConvertAll(methodInfo => new NonTestMethod(methodInfo, instance, true));
            newTestingClass.AfterMethods = FindMethodsByAttrInClass(type, typeof(After))
                .ConvertAll(methodInfo => new NonTestMethod(methodInfo, instance, false));

            var tests = FindMethodsByAttrInClass(type, typeof(MyTest));
            foreach (var test in tests)
            {
                var attr = GetTestAttribute(test);
                var a = (MyTest)attr;
                newTestingClass.TestMethods.Add(new TestMethod(test, instance, a.expected, a.ignore != "", a.ignore));
            }
        }
    }

    public void ExecuteTests(string path)
    {
        if (!Directory.Exists(path)) return;
        LoadTests(path);
        Parallel.ForEach(TestingClasses, testingClass => testingClass.RunTests());
        HasTested = true;
    }

    private static Attribute GetTestAttribute(MemberInfo methodInfo)
    {
        foreach (var attr in methodInfo.GetCustomAttributes())
        {
            if (attr.GetType().ToString() == typeof(MyTest).ToString())
            {
                return attr;
            }
        }

        throw new Exception("This method hasn't this attr");
    }

    private static bool HasAttr(MethodInfo methodInfo, Type attrType)
    {
        return methodInfo.GetCustomAttributes()
            .Any(attr => attr.GetType().ToString() == attrType.ToString());
    }

    private static List<MethodInfo> FindMethodsByAttrInClass(Type classType, Type attrType)
    {
        var methodInfos = classType.GetMethods()
            .Where(method => HasAttr(method, attrType))
            .ToList();
        return methodInfos;
    }
}