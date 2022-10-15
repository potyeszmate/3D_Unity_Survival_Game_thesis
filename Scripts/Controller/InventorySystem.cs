using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using System;
using SVS.InventorySystem;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour, ISavable
{
    public Action onInventoryStateChanged, OnStructureUse;

    private UiInventory uiInventory;

    private InventorySystemData inventoryData;

    public int playerStorageSize = 20;

    public InteractionManager interactionManager;
    // Items ID-s getters
    public bool WeaponEquipped { get => inventoryData.ItemEquipped;}
    public string EquippedWeaponId { get => inventoryData.EquippedItemId; }
    public bool InventoryVisible { get => uiInventory.IsInventoryVisible; }

    public StructureItemSO selectedStructureData = null;
    public int selectedStructureUiId = 0;

    private void Awake()
    {
        uiInventory = GetComponent<UiInventory>(); 
    }

    private void Start()
    {
        inventoryData = new InventorySystemData(playerStorageSize, uiInventory.HotbarElementsCount);
        inventoryData.updateHotbarCallback += UpdateHotbarHandler;
        // Drop button (not using currently)
        uiInventory.AssignDropButtonHandler(DropItemHandler);
        // Use button (I think i will dele it, because I will use the items just in the hotbar)
        uiInventory.AssignUseButtonHandler(UseInventoryItemHandler);

        var hotbarUiElementsList = uiInventory.GetUiElementsForHotbar();
        // Giving each hotbar slots the events and caclbacks ( onlcik,dragging )
        for (int i = 0; i < hotbarUiElementsList.Count; i++)
        {
            inventoryData.AddHotbarUiElement(hotbarUiElementsList[i].GetInstanceID());
            hotbarUiElementsList[i].OnClickEvent += UseHotbarItemHandler;
            hotbarUiElementsList[i].DragContinueCallback += DraggingHandler;
            hotbarUiElementsList[i].DragStartCallback += DragStartHandler;
            hotbarUiElementsList[i].DragStopCallback += DragStopHandler;
            hotbarUiElementsList[i].DropCalback += DropHandler;
        }
    }

    // This is when we  placeitem, then we of course dele it from our inventory
    internal void RemoveSelectedStructureFromInventory()
    {
        RemoveItemFromInventory(selectedStructureUiId);
        selectedStructureUiId = 0;
        selectedStructureData = null;
    }

    // If we craft, then we need to delete the ingredients from our panel 
    private void RemoveItemFromInventory(int ui_id)
    {
        inventoryData.TakeOneFromItem(ui_id);
        if (inventoryData.CheckIfSelectedItemIsEmpty(ui_id))
        {
            ClearUIElement(ui_id);
            inventoryData.RemoveItemFromInventory(ui_id);
        }
        else
        {
            UpdateUI(ui_id, inventoryData.GetItemCountFor(ui_id));
        }
        onInventoryStateChanged.Invoke();
    }

    // Crafting an item. We take items what we need, delete it, add the given item to the storage and update the panels
    internal void CraftAnItem(RecipeSO recipe)
    {
        foreach (var recipeIngredient in recipe.ingredientsRequired)
        {
            inventoryData.TakeFromItem(recipeIngredient.ingredient.ID, recipeIngredient.count);
        }
        inventoryData.AddToStorage(recipe);
        UpdateInventoryItems();
        UpdateHotbarHandler();
        onInventoryStateChanged.Invoke();
    }

    // We have to update the inventory items, beacause it always changes in the game 
    private void UpdateInventoryItems()
    {
        ToggleInventory();
        ToggleInventory();
    }
    // Checks if the inventory is full (important, we can use it for the backpack later)
    internal bool CheckInventoryFull()
    {
        return inventoryData.CheckIfStorageIsFull();
    }
    
    // Checks if we have enough number of the required itenm to craft
    internal bool CheckResourceAvailability(string id, int count)
    {
        return inventoryData.CheckItemInStorage(id, count);
    }

    // This method is responsible for the hotbars.
    internal void HotbarShortKeyHandler(int hotbarKey)
    {
        //var ui_index = hotbarKey == 0 ? 9 : hotbarKey - 1; ez az eredeti
        var ui_index = hotbarKey - 1;
        // This checks us the elements and the hotbar slot ID
        var uiElementID = uiInventory.GetHotbarElementUiIDWithIndex(ui_index);
        // If no elements then we dont have any item
        if (uiElementID == -1)
        {
            return;
        }
        // Gets the item ID 
        var id = inventoryData.GetItemIdFor(uiElementID);
        if (id == null)
            return;
        var itemData = ItemDataManager.instance.GetItemData(id);
        // Use the item
        UseItem(itemData, uiElementID);

    }
    // Will delete it, because I decideed to not use items from the inventory panel, just from the hotbar panel
    private void UseInventoryItemHandler()
    {
        Debug.Log("Use item");
        var itemData = ItemDataManager.instance.GetItemData(inventoryData.GetItemIdFor(inventoryData.selectedItemUIID));
        UseItem(itemData, inventoryData.selectedItemUIID);
    }

    // Responsible for Using the item (important method)
    private void UseItem(ItemSO itemData, int ui_id)
    {
        // If its a placable item
        if(itemData.GetItemType() == ItemType.Structure)
        {
            // Give the data to the structure type 
            selectedStructureUiId = ui_id;
            selectedStructureData = (StructureItemSO)itemData;
            // Go to placement state
            OnStructureUse.Invoke();
            return;
        }
        // If we using the item we remove it from inventory (because we dont want to use it more than once during using it)
        if (interactionManager.UseItem(itemData))
        {
            RemoveItemFromInventory(ui_id);
        }
        
        // Here ew equipping the item
        else if (interactionManager.EquipItem(itemData))
        {
            // deselecting any other selected item
            DeselectCurrentItem();
            // Not using this line right now, but it can be used when we spawn a single item to a hand (roght now we spawn the items woth the hand modell)
            ItemSpawnManager.instance.RemoveItemFromPlayerHand();
            if (inventoryData.ItemEquipped)
            {
                // selecting or deselecting the item
                uiInventory.ToggleEquipSelectedItem(inventoryData.EquippedUI_ID);
                if (inventoryData.EquippedUI_ID == ui_id)
                {
                    inventoryData.UnequipItem();
                    return;
                }
            }
            
            inventoryData.EquipItem(ui_id);
            uiInventory.ToggleEquipSelectedItem(ui_id);
            ItemSpawnManager.instance.CreateItemObjectInPlayerHand(itemData.ID);
        } 
        
    }
    // Alwys update the UI because of the item count
    private void UpdateUI(int ui_id, int count)
    {
        uiInventory.UpdateItemInfo(ui_id, count);
    }

    // this is a handler for dropping the item (when dropping, clear the item from the panel)
    private void DropItemHandler()
    {
        var id = inventoryData.selectedItemUIID;
        ItemSpawnManager.instance.CreateItemAtPlayersFeet(inventoryData.GetItemIdFor(id), inventoryData.GetItemCountFor(id));
        ClearUIElement(id);
        inventoryData.RemoveItemFromInventory(id);
        //onInventoryStateChanged.Invoke();
    }

    // This is just for the Use and Drop BUTTON(!), but currently neighter of them is used or active
    private void ClearUIElement(int ui_id)
    {
        uiInventory.DisableHighlightForSelectedItem(ui_id);
        uiInventory.ClearItemElement(ui_id);
        uiInventory.ToggleItemButtons(false, false);
    }

    // This method is responsible for the hotbar 
    private void UpdateHotbarHandler()
    {
        // Put the hotbar elements to a list (UI + data)
        var uiElementsList = uiInventory.GetUiElementsForHotbar();
        var hotbarItemsList = inventoryData.GetItemsDataForHotbar();
        // Go through the list
        for (int i = 0; i < uiElementsList.Count; i++)
        {   
            // Check every element and clear the item
            var uiItemElement = uiElementsList[i];
            uiItemElement.ClearItem();
            var itemData = hotbarItemsList[i];
            if (itemData.IsNull == false)
            {   
                // Initialize the name, count and the sprite
                var itemName = ItemDataManager.instance.GetItemName(itemData.ID);
                var itemSprite = ItemDataManager.instance.GetItemSprite(itemData.ID);
                uiItemElement.SetItemUI(itemName, itemData.Count, itemSprite);
            }
        }
    }

    // Method responsible for the usable items in the hotbar
    private void UseHotbarItemHandler(int ui_id, bool isEmpty)
    {
        Debug.Log("Useing Hotbar Item");
        if (isEmpty)
            return;
        // Deselect  the item if we have it equipped
        DeselectCurrentItem();
        var itemData = ItemDataManager.instance.GetItemData(inventoryData.GetItemIdFor(ui_id));
        // Use the item
        UseItem(itemData, ui_id);

    }

    // Toggle the inventory (when its not visible we prepare the inventory UI and the data )
    public void ToggleInventory()
    {
        if(uiInventory.IsInventoryVisible == false)
        {
            inventoryData.ResetSelectedItem();
            inventoryData.ClearInventoryUIElements();
            PrepareUI();
            PutDataInUI();
        }
        // If visible, then toggle it to unvisible
        uiInventory.ToggleUI();
    }

    //This method puts the item data to the UI of the items. The name, sprite and count 
    private void PutDataInUI()
    {
        var uiElementsList = uiInventory.GetUiElementsForInventory();
        var inventoryItemsList = inventoryData.GetItemsDataForInventory();
        // Goes through the UI list and puts the data to it from the DataManager (the item datas)
        for (int i = 0; i < uiElementsList.Count; i++)
        {
            var uiItemElement = uiElementsList[i];
            var itemData = inventoryItemsList[i];
            if (itemData.IsNull == false)
            {
                var itemName = ItemDataManager.instance.GetItemName(itemData.ID);
                var itemSprite = ItemDataManager.instance.GetItemSprite(itemData.ID);
                uiItemElement.SetItemUI(itemName, itemData.Count, itemSprite);
            }
            inventoryData.AddInventoryUiElement(uiItemElement.GetInstanceID());

        }
        // This is for the indicators. Check which panel spot needs the indicator to show (the equipped indicator if we equip item)
        for (int i = 0; i < uiElementsList.Count; i++)
        {
            var uiItemElement = uiElementsList[i];
            if (inventoryData.EquippedUI_ID == uiItemElement.GetInstanceID())
            {
                uiItemElement.ToggleEquippedIndicator();
            }
        }
    }

    // Prepares the UI when its not visible (sets its maximum number of storage)
    private void PrepareUI()
    {   
        
        uiInventory.PrepareInventoryItems(inventoryData.PlayerStorageLimit);
        AddEventHandlersToInventoryUiElements();

    }

    // We need this event handler because of the dragging and the clicking of the items (start dragging, stop dragging and clicking)
    private void AddEventHandlersToInventoryUiElements()
    {
        foreach (var uiItemElement in uiInventory.GetUiElementsForInventory())
        {
            // So thise are responsible for the dragging system
            uiItemElement.OnClickEvent += UiElementSelectedHandler;
            uiItemElement.DragContinueCallback += DraggingHandler;
            uiItemElement.DragStartCallback += DragStartHandler;
            uiItemElement.DragStopCallback += DragStopHandler;
            uiItemElement.DropCalback += DropHandler;
        }
    }

    // This method is responsible for the Dropping of the items when we dragging them
    private void DropHandler(PointerEventData eventData, int droppedItemID)
    {
        if(uiInventory.Draggableitem != null)
        {
            var draggedItemID = uiInventory.DraggableItemPanel.GetInstanceID();
            if (draggedItemID == droppedItemID)
            {
                return;
            }
            // In the start, we select the item by clicking on them (OnClick), but if we start dragging we have to deselect it
            // To know that we are in the dragging step
            DeselectCurrentItem();
            //
            if (uiInventory.CheckItemInInventory(draggedItemID))

            {
                if (uiInventory.CheckItemInInventory(droppedItemID))
                {
                    // If the item was in the inventory and we placed it to the inventory
                    DroppingItemsInventoryToInventory(droppedItemID, draggedItemID);
                }
                else
                {
                    // If the item was in the inventory and we did not placed it to the inventory
                    DroppingItemsInventoryToHotbar(droppedItemID, draggedItemID);
                }
            }
            else
            {
                // If the item was not in the inventory and we placed it to the inventory
                if (uiInventory.CheckItemInInventory(droppedItemID))
                {
                    DroppingItemsHotbarToInventory(droppedItemID, draggedItemID);
                }
                else
                {
                    // If the item was not in the inventory and we did not placed it to the inventory
                    DroppingItemsHotbarToHotbar(droppedItemID, draggedItemID);
                }
            }
            
        }
    }

    // Uses the UIInventory's Swapping methods. The funcion is explaind there 
    private void DroppingItemsHotbarToHotbar(int droppedItemID, int draggedItemID)
    {
        uiInventory.SwapUiItemHotbarToHotbar(droppedItemID, draggedItemID);
        inventoryData.SwapStorageItemsInsideHotbar(droppedItemID, draggedItemID);
    }

    private void DroppingItemsHotbarToInventory(int droppedItemID, int draggedItemID)
    {
        uiInventory.SwapUiItemHotbarToInventory(droppedItemID, draggedItemID);
        inventoryData.SwapStorageHotbarToInventory(droppedItemID, draggedItemID);
    }

    private void DroppingItemsInventoryToHotbar(int droppedItemID, int draggedItemID)
    {
        uiInventory.SwapUiItemInventoryToHotbar(droppedItemID, draggedItemID);
        inventoryData.SwapStorageInventoryToHotbar(droppedItemID, draggedItemID);
    }

    private void DroppingItemsInventoryToInventory(int droppedItemID, int draggedItemID)
    {
        uiInventory.SwapUiItemInventoryToInventory(droppedItemID, draggedItemID);
        inventoryData.SwapStorageItemsInsideInventory(droppedItemID, draggedItemID);
    }

    // this method deselects the item we clicked on
    private void DeselectCurrentItem()
    {
        if (inventoryData.selectedItemUIID != -1)
        {
            uiInventory.DisableHighlightForSelectedItem(inventoryData.selectedItemUIID);
            uiInventory.ToggleItemButtons(false, false);
        }
        inventoryData.ResetSelectedItem();
    }

    // Destroys the object we dragged when we placed it down
    private void DragStopHandler(PointerEventData eventData)
    {
        uiInventory.DestroyDraggedObject();
    }

    // Creates a draggable object (as it explained in the UIInventory), by copying the item and destroys the dragged object
    private void DragStartHandler(PointerEventData eventData, int ui_id)
    {
        uiInventory.DestroyDraggedObject();
        uiInventory.CreateDraggableItem(ui_id);
    }

    // Moves the draggable item
    private void DraggingHandler(PointerEventData eventData)
    {
        uiInventory.MoveDraggableItem(eventData);
    }

    // This method is responsible for the selecting item in the inventory
    private void UiElementSelectedHandler(int ui_id, bool isEmpty)
    {
        if (isEmpty)
            return;
        // Deselect the selected item if we click to an other item
        DeselectCurrentItem();
        // Set it to current selected, highlight it and toggle the buttons needed (we dont use the buttons now)
        inventoryData.SetSelectedItemTo(ui_id);
        uiInventory.HighlightSelectedItem(ui_id);
        uiInventory.ToggleItemButtons(ItemDataManager.instance.IsItemUsabel(inventoryData.GetItemIdFor(inventoryData.selectedItemUIID)),true);
        // Cant drop if equipped
        if (inventoryData.ItemEquipped)
        {
            // The point is that the ID of the Tool item currently in use will be inventoryData.EquippedUI_ID
            if(ui_id == inventoryData.EquippedUI_ID)
            {
                uiInventory.ToggleItemButtons(ItemDataManager.instance.IsItemUsabel(inventoryData.GetItemIdFor(inventoryData.selectedItemUIID)), false);
            }
        }
        
        
    }

    // Adds the given item to the storage
    public int AddToStorage(IInventoryItem item)
    {
        int val = inventoryData.AddToStorage(item);
        return val;
    }

    // Takes data from the inventory
    public string GetJsonDataToSave()
    {
        // Convert inventory data to Json string format 
        return JsonUtility.ToJson(inventoryData.GetDataToSave());
    }
    // Loads data into the inventory
    public void LoadJsonData(string jsonData)
    {   
        // The saved Json data is loaded
        SavedItemSystemData dataToLoad = JsonUtility.FromJson<SavedItemSystemData>(jsonData);
        inventoryData.LoadData(dataToLoad);
    }


}

