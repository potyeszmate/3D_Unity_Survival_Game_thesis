using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Generic Food Item", menuName ="ItemSO/WaterItemSO")]
public class WaterItemSO : ItemSO
{
    public int thirstBonus = 0, healthBoost = 0, hungerBonus = 0, energyBonus = 0;

    // Water is usable
    public override bool IsUsable()
    {
        return true;
    }

}
