using Entropia.World;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WorldChunkInstantiator : Instantiator
{
    public WorldChunkMono Spawn(WorldChunk chunk)
    {
        GameObject obj = Spawn(
            prefabName: "Chunk",
            deltapos: chunk.Sector.Center(),
            hierarchyName: $"Chunk {chunk.Sector}"
        );

        if (!obj.TryGetComponent<WorldChunkMono>(out var instance))
            throw new KeyNotFoundException($"No component of type '{typeof(WorldChunkMono)}' was found");

        instance.Recycle(chunk);

        return instance;
    }

    public void Despawn(WorldChunkMono instance)
    {
        instance.Abandon();

        Despawn(instance.gameObject);
    }
}
