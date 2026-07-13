using Entropia.Worldgen;
using UnityEngine;

public class AsteroidMono : WorldFeatureMono<Asteroid>
{
    protected override void Initialize(Asteroid asteroid)
    {
        transform.localScale = asteroid.Size * Vector3.one;
    }
}
