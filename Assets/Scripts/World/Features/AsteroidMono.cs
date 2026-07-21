using Entropia.World.Features;
using NoEntropy;
using UnityEngine;

public partial class AsteroidMono : WorldFeatureMono<Asteroid>
{
    [SerializeField] [NullCheck] private MaterialList m_Materials;
    [SerializeField] [NullCheck] private MeshRenderer m_MeshRenderer;

    protected override void Initialize(Asteroid asteroid)
    {
        transform.localScale = asteroid.Size * Vector3.one;
        m_MeshRenderer.sharedMaterial = m_Materials.GetByName(asteroid.Type.ToString());
    }
}
