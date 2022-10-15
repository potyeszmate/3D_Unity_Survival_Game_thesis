using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generic Weapon Item", menuName = "ItemSO/WeaponItemSO")]
public class WeponItemSO : ItemSO
{

    
    public override bool IsUsable()
    {
        return true;
    }

}
