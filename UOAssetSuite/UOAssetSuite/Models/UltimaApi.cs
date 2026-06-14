using System.Reflection;

namespace UOAssetSuite.Models;

public sealed class UltimaApi
{
    private static readonly Lazy<Assembly> UltimaAssembly = new(LoadUltimaAssembly);

    public UltimaApi(string? clientPath = null)
    {
        if (!string.IsNullOrWhiteSpace(clientPath))
        {
            SetClientPath(clientPath);
        }
    }

    public void SetClientPath(string clientPath)
    {
        InvokeStatic("Ultima.Files", "SetMulPath", clientPath);
    }

    public object? InvokeStatic(string typeName, string methodName, params object?[] args)
    {
        var type = FindType(typeName);
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == methodName && m.GetParameters().Length == args.Length);

        foreach (var method in methods)
        {
            var converted = TryConvertArguments(method, args);
            if (converted is not null)
            {
                return method.Invoke(null, converted);
            }
        }

        throw new MissingMethodException(type.FullName, methodName);
    }


    public object? InvokeFirstAvailable(string typeName, params string[] methodNames)
    {
        var type = FindType(typeName);
        foreach (var methodName in methodNames)
        {
            var method = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 0);
            if (method is not null)
            {
                return method.Invoke(null, Array.Empty<object?>());
            }
        }

        throw new MissingMethodException(type.FullName, string.Join("/", methodNames));
    }

    public object? ReadStaticProperty(string typeName, string propertyName)
    {
        var property = FindType(typeName).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static);
        if (property is null)
        {
            throw new MissingMemberException(typeName, propertyName);
        }

        return property.GetValue(null);
    }

    public object? ReadIndexedValue(string typeName, string propertyName, int index)
    {
        var value = ReadStaticProperty(typeName, propertyName);
        return value switch
        {
            Array array => array.GetValue(index),
            System.Collections.IList list => list[index],
            _ => throw new InvalidOperationException($"Ultima.{propertyName} is not an indexed collection.")
        };
    }

    public static T? GetProperty<T>(object? instance, string propertyName)
    {
        if (instance is null)
        {
            return default;
        }

        var property = instance.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (property is null)
        {
            return default;
        }

        var value = property.GetValue(instance);
        if (value is T typed)
        {
            return typed;
        }

        return value is null ? default : (T)Convert.ChangeType(value, typeof(T));
    }

    private static Type FindType(string typeName)
    {
        var type = Type.GetType(typeName)
            ?? AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType(typeName, false))
                .FirstOrDefault(t => t is not null)
            ?? UltimaAssembly.Value.GetType(typeName, true);

        return type ?? throw new InvalidOperationException($"Could not find {typeName}. Ensure Resources\\Ultima.dll is copied with the application.");
    }

    private static Assembly LoadUltimaAssembly()
    {
        var resourcePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Ultima.dll");
        if (File.Exists(resourcePath))
        {
            return Assembly.LoadFrom(resourcePath);
        }

        return Assembly.Load("Ultima");
    }

    private static object?[]? TryConvertArguments(MethodInfo method, object?[] args)
    {
        var parameters = method.GetParameters();
        var converted = new object?[args.Length];
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            var targetType = parameters[i].ParameterType;
            if (arg is null || targetType.IsInstanceOfType(arg))
            {
                converted[i] = arg;
            }
            else
            {
                try
                {
                    converted[i] = Convert.ChangeType(arg, targetType);
                }
                catch
                {
                    return null;
                }
            }
        }

        return converted;
    }
}
