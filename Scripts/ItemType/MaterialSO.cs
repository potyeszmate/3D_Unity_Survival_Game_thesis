using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="MaterialItemData", menuName ="ItemSO/MaterialSO")]
public class MaterialSO : ItemSO
{
    public ResourceType resourceType;
    private void OnEnable()
    {
        itemType = ItemType.Material;
    }
}

// The materials Type. Currenty we have to materials
public enum ResourceType
{
    None,
    Wood,
    Stone
}
