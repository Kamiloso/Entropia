using Entropia.World.Features;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class WorldFeatureMono : MonoBehaviour
{
    public abstract void Initialize(WorldFeature worldFeature);
}

public abstract class WorldFeatureMono<T> : WorldFeatureMono where T : WorldFeature
{
    protected abstract void Initialize(T worldFeature);

    public override void Initialize(WorldFeature worldFeature)
    {
        if (!typeof(T).IsAssignableFrom(worldFeature.GetType()))
        {
            throw new ArgumentException(
                $"Invalid world feature has been provided " +
                $"(expected {typeof(T)}, received {worldFeature.GetType()})");
        }

        Initialize((T)worldFeature);
    }
}
