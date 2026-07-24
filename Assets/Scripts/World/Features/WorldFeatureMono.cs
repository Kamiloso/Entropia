using Entropia.Core;
using Entropia.World.Features;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class WorldFeatureMono : MonoBehaviour, IRecyclable<WorldFeature>
{
    public virtual void Recycle(WorldFeature worldFeature) { }
    public virtual void Abandon() { }
}

[DisallowMultipleComponent]
public abstract class WorldFeatureMono<T> : WorldFeatureMono, IRecyclable<T>
    where T : WorldFeature
{
    public virtual void Recycle(T worldFeature) { }

    public override void Recycle(WorldFeature worldFeature)
    {
        if (!typeof(T).IsAssignableFrom(worldFeature.GetType()))
        {
            throw new ArgumentException(
                $"Invalid world feature has been provided " +
                $"(expected {typeof(T)}, received {worldFeature.GetType()})");
        }

        Recycle((T)worldFeature);
    }
}
