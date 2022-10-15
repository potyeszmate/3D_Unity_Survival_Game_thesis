using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPanelHelper : MonoBehaviour
{

    public Image itemImage;
    [SerializeField]
    protected Text nameText = null;
    public string itemName;
    public Outline outline;

    public Sprite backgroundSprite;

    protected virtual void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        
        // clear the item if the sprite is the bacground (if we dont have the item we clear it)
        if (itemImage.sprite == backgroundSprite)
        {
            ClearItem();
        }
        
    }

    // Sets the items datas 
    public virtual void SetItemUI(string name, Sprite image)
    {
        itemName = name;
        nameText.text = itemName;
        SetImageSprite(image);
    }

    // Sets the items sprite to the image we want
    protected virtual void SetImageSprite(Sprite image)
    {
        itemImage.sprite = image;
    }

    // Clearing the item UI
    public virtual void ClearItem()
    {
        itemName = "";
        nameText.text = itemName;
        ResetImage();
        ToggleHighlight(false);
    }

    // Toggle the outline of the item
    protected virtual void ToggleHighlight(bool val)
    {
        outline.enabled = val;
    }

    // When we clear the item we have to reset it
    protected virtual void ResetImage()
    {
        itemImage.sprite = backgroundSprite;
    }
    
    // We need puplic and protected moethod of the outline changer
    public virtual void ToggleHoghlight(bool val)
    {
        outline.enabled = val;
    }
}
