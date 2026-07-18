using UnityEngine;
using VContainer;

public class AutoInjector : MonoBehaviour
{
    [Inject]
    private void Construct(IObjectResolver objectResolver)
    {
        var scripts = FindObjectsByType<MonoBehaviour>();

        foreach (var obj in scripts)
        {
            if (obj == this) continue;

            objectResolver.Inject(obj);
        }
    }
}
