using Entropia.World.Features;
using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(MeshRenderer))]
public class AsteroidMono : WorldFeatureMono<Asteroid>
{
    [SerializeField] private MaterialList m_Materials;
    [SerializeField] private MeshRenderer m_MeshRenderer;

    [Inject]
    private void Construct()
    {
        if (m_Materials == null)
            throw new ArgumentNullException(nameof(m_Materials));

        if (m_MeshRenderer == null)
            throw new ArgumentNullException(nameof(m_MeshRenderer));
    }

    protected override void Initialize(Asteroid asteroid)
    {
        transform.localScale = asteroid.Size * Vector3.one;
        m_MeshRenderer.sharedMaterial = m_Materials.GetByName(asteroid.Type.ToString());
    }
}
