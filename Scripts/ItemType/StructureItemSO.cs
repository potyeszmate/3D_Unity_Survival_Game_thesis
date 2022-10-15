using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="StructureItemData", menuName ="ItemSO/StructureItemSO")]
public class StructureItemSO : ItemSO
{
    [SerializeField]
    protected GameObject prefab;

    private void OnEnable()
    {
        itemType = ItemType.Structure;
    }
    // The structure is always a usable item
    public override bool IsUsable()
    {
        return true;
    }

    // For a structere, we need its prefab to place it down. It gives us the items prefab
    public override GameObject GetPrefab()
    {
        return prefab == null ? model : prefab;
    }
}
