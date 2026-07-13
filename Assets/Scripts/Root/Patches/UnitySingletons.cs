using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IUnitySingleton { }
public interface IPreUnitySingleton : IUnitySingleton { }
public interface INoExceptUnitySingleton : IUnitySingleton { }

public static class UnitySingletons
{
    private static GameObject _obj;

    public static T Get<T>() where T : MonoBehaviour, IUnitySingleton
    {
        return _obj.GetComponent<T>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        try
        {
            _obj = new GameObject("EternalObject");

            Object.DontDestroyOnLoad(_obj);

            List<Type> types = GetLoadableTypes(typeof(UnitySingletons).Assembly)
                .Where(t =>
                    typeof(MonoBehaviour).IsAssignableFrom(t) &&
                    typeof(IUnitySingleton).IsAssignableFrom(t) &&
                    !t.IsAbstract)
                .OrderByDescending(typeof(INoExceptUnitySingleton).IsAssignableFrom)
                .ThenByDescending(typeof(IPreUnitySingleton).IsAssignableFrom)
                .ToList();

            foreach (Type type in types)
            {
                _obj.AddComponent(type);
            }
        }
        catch
        {
            Process.GetCurrentProcess().Kill();
        }
    }

    private static Type[] GetLoadableTypes(Assembly assembly)
    {
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null).ToArray();
        }
    }
}
