using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject, ISerializationCallbackReceiver
{
    public string ID;
    public string itemName;
    public Sprite imageSprite;
    [SerializeField]
    protected GameObject model;
    public bool isStackable;
    [Range(1,100)]
    public int stackLimit = 100;
    public ItemType itemType;


    public void OnAfterDeserialize()
    {
    }

    // Before the items serialization (gives a default name to the item, which is the modells name)
    public void OnBeforeSerialize()
    {
        if (string.IsNullOrEmpty(this.ID))
        {
            this.ID = Guid.NewGuid().ToString("N");
        }
        if (string.IsNullOrEmpty(itemName) && model != null)
        {
            itemName = model.name;
        }
    }

    // Return the item image
    public Sprite GetImage()
    {
        return imageSprite;
    }

    // Return the item type
    public ItemType GetItemType()
    {
        return itemType;
    }

    // Return if item is IUsable (its false in default)
    public virtual bool IsUsable()
    {
        return false;
    }

    // Return the items prefab
    public virtual GameObject GetPrefab()
    {
        return model;
    }

    // Return the items modell
    public GameObject GetModel()
    {
        return model;
    }
}

// These are our items types
public enum ItemType
{
    None,
    // Foot type
    Food,
    //Drink
    Water,
    // Weapon type
    Weapon,
    // Material type
    Material,
    // Structure type
    Structure
}
