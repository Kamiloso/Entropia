using UnityEngine;

[CreateAssetMenu(fileName = "PrefabList", menuName = "Custom Data/Prefab List")]
public class PrefabList : AssetList<GameObject>
{
    protected override string AssetTypeName => "Prefab";
}
