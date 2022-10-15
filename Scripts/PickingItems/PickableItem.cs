using SVS.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickable, IInventoryItem
{
    // ITemSO references
    [SerializeField]
    public ItemSO dataSource;
    public int count;

    public string ID => dataSource.ID;

    GameObject thisItem;

    private void Start() 
    {
        thisItem = transform.gameObject;    
    }

    public int Count { 
        // It checks if it can be stacked.If it can, it should be counted, if not, it returns 1, because there can only be 1 in 1 slot.
        get 
        {
            if (dataSource.isStackable == false)
            {
                return 1;
            }
            return count;
        } 
    }
    // Stackable or not
    public bool IsStackable => dataSource.isStackable;
    // The maximum stack limit of the item
    public int StackLimit => dataSource.stackLimit;
    // Returns with the picked up item
    public IInventoryItem PickUp()
    {
        return this;
    }
    // When I pick up an item, the method checks the quantity and deletes it if it is 0 (i.e. if there are no more that we can pick up)
    public void SetCount(int value)
    {
        count = value;
        if (count == 0)
        {
            thisItem.SetActive(false);
        }
    }
}
