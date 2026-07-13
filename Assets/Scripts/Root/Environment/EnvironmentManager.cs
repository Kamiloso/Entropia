using Entropia.Root.Lifetime;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace Entropia.Root.Environment;

[Preserve]
public class EnvironmentManager : MonoBehaviour, IUnitySingleton
{
    // TODO: rethink it and refactor

    // TODO: make a scriptable object for scenes or sth
    private const string GAME = "Game";
    private const string MENU = "Menu";

    private ServiceLocator _services = new();
    private bool _registered = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += (_, _) => PrepareServices();
        SceneManager.sceneUnloaded += _ => ResetServices();
        Application.quitting += ResetServices;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(GAME);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(MENU);
    }

    public T GetService<T>() where T : notnull
    {
        PrepareServices();

        return _services.Get<T>();
    }

    private void PrepareServices()
    {
        if (_registered) return;
        _registered = true;

        _services.RegisterBaseModels();

        switch (SceneManager.GetActiveScene().name)
        {
            case GAME: _services.RegisterGameModels(); break;
            case MENU: _services.RegisterMenuModels(); break;

            default: throw new NotImplementedException();
        }
    }

    private void ResetServices()
    {
        _services.Dispose();
        _services = new ServiceLocator();
        _registered = false;
    }
}
