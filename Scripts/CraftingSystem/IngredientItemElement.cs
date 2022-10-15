using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientItemElement : ItemPanelHelper
{
    public Image panelImage;
    public Text countNeedeTxt;

    // Change the color of the panel image
    private void ModifyPanelAlpha(int value)
    {
        var panelColor = panelImage.color;
        panelColor.a = Mathf.Clamp01(value);
        panelImage.color = panelColor;
    }

    // If item is available  (craftable) -> Red color
    public void SetUnavailable()
    {
        ModifyPanelAlpha(1);
    }

    // If item is available  (craftable) -> No color
    public void ResetAvailability()
    {
        ModifyPanelAlpha(0);
    }

    // Sets the UI (needed number of ingedients), but we need to override it because every craftable item has different ingredients and number 
    public override void SetItemUI(string name, Sprite image)
    {
        base.SetItemUI(name, image);
        countNeedeTxt.text = "x 0";
    }

    // Sets the item UI
    public void SetItemUI(string name, Sprite image, int count, bool enoughItems)
    {
        base.SetItemUI(name, image);
        countNeedeTxt.text = "x "+count;
        if (enoughItems)
        {
            ResetAvailability();
        }
        else
        {
            SetUnavailable();
        }
    }
}
