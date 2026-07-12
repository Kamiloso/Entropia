#nullable disable
using Entropia.Root;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ExtendedMonoBehaviour : MonoBehaviour
{
    private static ServiceLocator _services = new();
    private static bool _registered = false;

    protected static T Ref<T>()
    {
        PrepareServices();

        return _services.Get<T>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        SceneManager.sceneUnloaded += _ => ResetServices();
    }

    private static void PrepareServices()
    {
        if (_registered) return;
        _registered = true;

        _services.RegisterBaseModels();

        switch (SceneManager.GetActiveScene().path)
        {
            case "Assets/Scenes/Game.unity": _services.RegisterGameModels(); break;
            case "Assets/Scenes/Menu.unity": _services.RegisterMenuModels(); break;
            default: throw new NotImplementedException();
        }
    }

    private static void ResetServices()
    {
        _services.Dispose();
        _services = new ServiceLocator();
        _registered = true;
    }
}
