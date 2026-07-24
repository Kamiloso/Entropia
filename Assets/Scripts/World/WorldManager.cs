using Entropia.Scenes;
using Entropia.Structs;
using Entropia.World;
using Entropia.World.Spy;
using NoEntropy;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[DisallowMultipleComponent]
[Resolve(typeof(ShiftRoot))]
[Resolve(typeof(IWorldProvider))]
[Resolve(typeof(ISectorSpy))]
[UseComponent(typeof(WorldChunkInstantiator))]
public partial class WorldManager : MonoBehaviour
{
    private readonly HashSet<Sector3> _loadedSectors = new();
    private readonly HashSet<Sector3> _obtainingSectors = new();
    private readonly Dictionary<Sector3, WorldChunkMono> _activeChunks = new();

    private readonly CancellationTokenSource _cts = new();

    partial void OnConstruct()
    {
        SectorSpy.OnLoad += OnLoad;
        SectorSpy.OnUnload += OnUnload;
    }

    private void OnDestroy()
    {
        SectorSpy.OnLoad -= OnLoad;
        SectorSpy.OnUnload -= OnUnload;

        _cts.Cancel();
        _cts.Dispose();
    }

    private void Update()
    {
        SectorSpy.UpdatePosition(ShiftRoot.Shift.Position);
    }

    private void OnLoad(Sector3 sector)
    {
        _loadedSectors.Add(sector);
        if (!_obtainingSectors.Contains(sector))
        {
            StartCoroutine(ActivateChunkAsync(sector));
        }
    }

    private IEnumerator ActivateChunkAsync(Sector3 sector)
    {
        _obtainingSectors.Add(sector);

        Task<WorldChunk> chunkLoading = WorldProvider.ObtainWorldChunk(sector, _cts.Token);
        yield return new WaitUntil(() => chunkLoading.IsCompleted);

        _obtainingSectors.Remove(sector);

        try
        {
            WorldChunk chunk = chunkLoading.Result;

            if (!_loadedSectors.Contains(sector))
                yield break;

            if (chunk.Features.Length == 0)
                yield break;

            _activeChunks.Add(sector, WorldChunkInstantiator.Spawn(chunk));
        }
        catch (ExitException)
        {
            // TODO: implement world exit scenario
            Application.Quit();
        }
    }

    private void OnUnload(Sector3 sector)
    {
        _loadedSectors.Remove(sector);
        if (_activeChunks.Remove(sector, out WorldChunkMono instance))
        {
            WorldChunkInstantiator.Despawn(instance);
        }
    }
}
