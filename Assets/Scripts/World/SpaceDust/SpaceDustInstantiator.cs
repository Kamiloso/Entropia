using Entropia.Structs;
using UnityEngine;

public class SpaceDustInstantiator : Instantiator
{
    [SerializeField] PrefabList PrefabList;

    protected override bool UsePooling => true;

    protected override GameObject GetPrefabByName(string prefabName) =>
        PrefabList.GetByName(prefabName);

    public GameObject Spawn(Sector3 sector)
    {
        return Spawn(
            prefabName: "SpaceDust",
            deltapos: sector.Center(),
            rotation: Vec3.Zero
        );
    }

    public new void Despawn(GameObject obj)
    {
        base.Despawn(obj);
    }
}
