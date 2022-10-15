using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="ToolItemData", menuName ="ItemSO/ToolSO")]
public class ToolSO : WeponItemSO
{

    private void OnEnable()
    {
        // Wapon type object
        itemType = ItemType.Weapon;
    }


}


