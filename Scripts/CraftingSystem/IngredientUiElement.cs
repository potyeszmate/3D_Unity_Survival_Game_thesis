using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUiElement : ItemPanelHelper
{
    // The image of the ingredient (image)
    public Image panelImage; 
    // The ingredient count number (text)
    public Text countNeedeTxt;

    // It will change the colour of the matching panel, if there are enough or not enough ingredients
    private void ModifyPanelAlpha(int value)
    {
        var panelColor = panelImage.color;
        panelColor.a = Mathf.Clamp01(value);
        panelImage.color = panelColor;
        
    }
    // If there are not enough ingredients in the inventory for the craftable item, this method will be called
    public void SetUnavailable()
    {
        ModifyPanelAlpha(1);
    }
    // If there are enough ingredients in the inventory for the craftable item, this method will be called
    public void ResetAvailability()
    {
        ModifyPanelAlpha(0);
    }
    // Override the SetItemUI ( because it will change by the amount of needed items)
    public override void SetItemUI(string name, Sprite image)
    {
        base.SetItemUI(name, image);
        countNeedeTxt.text = "x 0";
    }
    // This method will be responsible for the operation of the ingredients
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
