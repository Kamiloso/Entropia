using Entropia.World.Features;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class AsteroidMono : WorldFeatureMono<Asteroid>
{
    [SerializeField] MaterialList Materials;
    [SerializeField] MeshRenderer MeshRenderer;

    protected override void Initialize(Asteroid asteroid)
    {
        transform.localScale = asteroid.Size * Vector3.one;
        MeshRenderer.sharedMaterial = Materials.GetByName(asteroid.Type.ToString());
    }
}
