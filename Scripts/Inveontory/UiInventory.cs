using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    public GameObject inventoryGeneralPanel;

    public UIStorageButtonsHelper uiStorageButtonHelper;

    public bool IsInventoryVisible { get => inventoryGeneralPanel.activeSelf; }
    public int HotbarElementsCount { get =>hotbarUiItems.Count;}
    public RectTransform Draggableitem { get => draggableitem; }
    public InventoryItemPanelHelper DraggableItemPanel { get => draggableItemPanel; }

    public Dictionary<int, InventoryItemPanelHelper> inventoryUiItems = new Dictionary<int, InventoryItemPanelHelper>();
    public Dictionary<int, InventoryItemPanelHelper> hotbarUiItems = new Dictionary<int, InventoryItemPanelHelper>();

    private List<int> listOfHotbarElementsID = new List<int>();

    public List<InventoryItemPanelHelper> GetUiElementsForHotbar()
    {
        return hotbarUiItems.Values.ToList();
    }

    public GameObject hotbarPanel, storagePanel;

    public GameObject storagePrefab;

    private RectTransform draggableitem;
    private InventoryItemPanelHelper draggableItemPanel;

    public Canvas canvas;

    private void Awake()
    {
        // When we start the game we hide the cursor
        HideMouseCursor();
        // Sets the inventorypanel false when we start
        inventoryGeneralPanel.SetActive(false);
        // Sets the hotbar elements
        foreach (Transform child in hotbarPanel.transform)
        {
            InventoryItemPanelHelper helper = child.GetComponent<InventoryItemPanelHelper>();
            if (helper != null)
            {
                hotbarUiItems.Add(helper.GetInstanceID(), helper);
                helper.isHotbarItem = true;
            }
        }
        listOfHotbarElementsID = hotbarUiItems.Keys.ToList();
        


    }

    //Toggles the inventpry panel on and off (if its off than on if its on than on)
    public void ToggleUI()
    {
        if(inventoryGeneralPanel.activeSelf == false)
        {
            inventoryGeneralPanel.SetActive(true);
            ShowMouseCursor();
        }
        else
        {
            inventoryGeneralPanel.SetActive(false);
            HideMouseCursor();
            DestroyDraggedObject();
            
        }
        uiStorageButtonHelper.HideAllButons();
    }

    // Lists the items that are in the UI
    internal int GetHotbarElementUiIDWithIndex(int ui_index)
    {
        if(listOfHotbarElementsID.Count <= ui_index)
        {
            return -1;
        }
        return listOfHotbarElementsID[ui_index];
    }

    // Use button in the inventroy (I dont use it, becasue i want to use the items just from the hotbar right now)
    public void AssignUseButtonHandler(Action handler)
    {
        uiStorageButtonHelper.OnUseBtnClick += handler;
    }

    // Clears the certain Item
    internal void ClearItemElement(int ui_id)
    {
        GetItemFromCorrectDictionary(ui_id).ClearItem();
    }

    // Returns with the item with the given ID
    private InventoryItemPanelHelper GetItemFromCorrectDictionary(int ui_id)
    {
        if (inventoryUiItems.ContainsKey(ui_id))
        {
            return inventoryUiItems[ui_id];
        }
        else if (hotbarUiItems.ContainsKey(ui_id))
        {
            return hotbarUiItems[ui_id];
        }
        return null;
    }

    // Updates the given items count
    internal void UpdateItemInfo(int ui_id, int count)
    {
        GetItemFromCorrectDictionary(ui_id).UpdateCount(count);
    }

    // Drops the item
    public void AssignDropButtonHandler(Action handler)
    {
        uiStorageButtonHelper.OnDropBtnClick += handler;
    }

    // Show the Drop and Use botten if needed (right now we dont show them, but we show the use just for the usable items)
    public void ToggleItemButtons(bool useBtn, bool dropButton)
    {
        uiStorageButtonHelper.ToggleDropButton(dropButton);
        uiStorageButtonHelper.ToggleUseButton(useBtn);
    }

    // Prepares the inventory items -> cleares the storagepanel objects then checks the element and add the items
    public void PrepareInventoryItems(int playerStorageLimit)
    {   
        for (int i = 0; i < playerStorageLimit; i++)
        {
            foreach (Transform child in storagePanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
        inventoryUiItems.Clear();
        for (int i = 0; i < playerStorageLimit; i++)
        {
            var element = Instantiate(storagePrefab, Vector3.zero, Quaternion.identity, storagePanel.transform);
            
            var itemHelper = element.GetComponent<InventoryItemPanelHelper>();
            inventoryUiItems.Add(itemHelper.GetInstanceID(), itemHelper);
        }

    }

    // Adds the items to the dictionary
    public List<InventoryItemPanelHelper> GetUiElementsForInventory()
    {
        return inventoryUiItems.Values.ToList();
    }

    // Toggles the selected items indicator (if its equipped, it will show the indicator)
    internal void ToggleEquipSelectedItem(int itemID)
    {
        if (hotbarUiItems.ContainsKey(itemID))
        {
            hotbarUiItems[itemID].ToggleEquippedIndicator();
        }
        else
        {
            inventoryUiItems[itemID].ToggleEquippedIndicator();
        }
    }

    // Destroyes the Dragged game object if we place it while dragging
    public void DestroyDraggedObject()
    {
        if(draggableitem != null)
        {
            Destroy(draggableitem.gameObject);
            draggableItemPanel = null;
            draggableitem = null;
        }
    }

    // Creates the same item if we want to drag something. We drag a copy of the item
    public void CreateDraggableItem(int ui_id)
    {
        if (CheckItemInInventory(ui_id))
        {
            draggableItemPanel = inventoryUiItems[ui_id];
        }
        else
        {
            draggableItemPanel = hotbarUiItems[ui_id];
        }

        Image itemImage = draggableItemPanel.itemImage;
        var imageObject = Instantiate(itemImage, itemImage.transform.position, Quaternion.identity, canvas.transform);
        imageObject.raycastTarget = false;
        imageObject.sprite = itemImage.sprite;

        draggableitem = imageObject.GetComponent<RectTransform>();
        draggableitem.sizeDelta = new Vector2(100, 100);

    }

    // Checks if item is in inventory (so if we pick up an existed item in our inventory then it will added to the same place, += count)
    public bool CheckItemInInventory(int ui_id)
    {
        return inventoryUiItems.ContainsKey(ui_id);
    }

    // Moves the dragged item
    internal void MoveDraggableItem(PointerEventData eventData)
    {
        var valueToAdd = eventData.delta / canvas.scaleFactor;
        draggableitem.anchoredPosition += valueToAdd;
    }

    // Responsible for the swapping in the inventory.
    // We use a temp item and then the temp item will be the real item when we place it down and then we delelete the other item
    internal void SwapUiItemInventoryToInventory(int droppedItemID, int draggedItemID)
    {
        var tempName = inventoryUiItems[draggedItemID].itemName;
        var tempCount = inventoryUiItems[draggedItemID].itemCount;
        var tempSprite = inventoryUiItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = inventoryUiItems[draggedItemID].isEmpty;

        var droppedItemData = inventoryUiItems[droppedItemID];
        inventoryUiItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount, droppedItemData.itemImage.sprite, droppedItemData.isEmpty);

        inventoryUiItems[droppedItemID].SwapWithData(tempName, tempCount, tempSprite, tempisEmpty);

        DestroyDraggedObject();
    }

    // The same as up just for the hotbar panel switch
    internal void SwapUiItemHotbarToHotbar(int droppedItemID, int draggedItemID)
    {
        var tempName = hotbarUiItems[draggedItemID].itemName;
        var tempCount = hotbarUiItems[draggedItemID].itemCount;
        var tempSprite = hotbarUiItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = hotbarUiItems[draggedItemID].isEmpty;

        var droppedItemData = hotbarUiItems[droppedItemID];
        hotbarUiItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount, droppedItemData.itemImage.sprite, droppedItemData.isEmpty);

        hotbarUiItems[droppedItemID].SwapWithData(tempName, tempCount, tempSprite, tempisEmpty);

        DestroyDraggedObject();
    }

    // The same as up, just for the switch between the hotbar and the inventory panel
    internal void SwapUiItemHotbarToInventory(int droppedItemID, int draggedItemID)
    {
        var tempName = hotbarUiItems[draggedItemID].itemName;
        var tempCount = hotbarUiItems[draggedItemID].itemCount;
        var tempSprite = hotbarUiItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = hotbarUiItems[draggedItemID].isEmpty;

        var droppedItemData = inventoryUiItems[droppedItemID];
        hotbarUiItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount, droppedItemData.itemImage.sprite, droppedItemData.isEmpty);

        inventoryUiItems[droppedItemID].SwapWithData(tempName, tempCount, tempSprite, tempisEmpty);

        DestroyDraggedObject();
    }

    // The same as up, just for the switch between the inventory and the hotbar panel
    internal void SwapUiItemInventoryToHotbar(int droppedItemID, int draggedItemID)
    {
        var tempName = inventoryUiItems[draggedItemID].itemName;
        var tempCount = inventoryUiItems[draggedItemID].itemCount;
        var tempSprite = inventoryUiItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = inventoryUiItems[draggedItemID].isEmpty;

        var droppedItemData = hotbarUiItems[droppedItemID];
        inventoryUiItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount, droppedItemData.itemImage.sprite, droppedItemData.isEmpty);

        hotbarUiItems[droppedItemID].SwapWithData(tempName, tempCount, tempSprite, tempisEmpty);

        DestroyDraggedObject();
    }

    // Highlights the selected item
    public void HighlightSelectedItem(int ui_id)
    {
        if (hotbarUiItems.ContainsKey(ui_id))
        {
            return;
        }
        inventoryUiItems[ui_id].ToggleHoghlight(true);
    }

    // Is inactivates the highlight
    public void DisableHighlightForSelectedItem(int ui_id)
    {
        
        if (hotbarUiItems.ContainsKey(ui_id))
        {
            //hotbarUiItems[ui_id].ToggleHoghlight(false);
            return;
        }
        inventoryUiItems[ui_id].ToggleHoghlight(false);
    }

    // Hide the mouse
    public void HideMouseCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Show the mouse
    public void ShowMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}

