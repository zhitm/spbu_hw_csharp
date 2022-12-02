using System.Reflection;
using Attributes;

namespace TestRunner;

public class TestRunner
{
    private readonly List<TestingClass> _testingClasses = new();

    private void LoadTests(string path)
    {
        Assembly assembly =
            Assembly.LoadFrom(path);
        foreach (var type in assembly.ExportedTypes)
        {
            if (!type.IsClass) continue;
            var newTestingClass = new TestingClass(type);
            var instance = Activator.CreateInstance(type);

            _testingClasses.Add(newTestingClass);
            newTestingClass.BeforeMethods = FindMethodsByAttrInClass(type, typeof(Before))
                .ConvertAll(methodInfo => new NonTestMethod(methodInfo, instance));
            newTestingClass.AfterMethods = FindMethodsByAttrInClass(type, typeof(After))
                .ConvertAll(methodInfo => new NonTestMethod(methodInfo, instance));

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
        LoadTests(path);
        foreach (var testingClass in _testingClasses)
        {
            testingClass.RunTests();
        }
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