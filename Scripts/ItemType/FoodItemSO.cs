using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Generic Food Item", menuName ="ItemSO/FoodItemSO")]
public class FoodItemSO : ItemSO
{
    // the bonuses we will add to the stat for using the certain item
    public int thirstBonus = 0, healthBoost = 0, hungerBonus = 0;

    // The food item will be usable (when use we add a certain value to our stat)
    public override bool IsUsable()
    {
        return true;
    }

}
