using SVS.InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventorySystemData
    {   
        
        // The other codes in the inventory system are built to these Storage code
        

        public Action updateHotbarCallback;
        private Storage storagePlayer, storageHotbar;
        List<int> inventoryUiElementIdList = new List<int>();
        List<int> hotbarUiElementIdList = new List<int>();

        public int selectedItemUIID = -1;
        
        // Sets the players and hotbars storage size
        public InventorySystemData(int playerStroageSize, int hotbarStorageSize)
        {
            storagePlayer = new Storage(playerStroageSize);
            storageHotbar = new Storage(hotbarStorageSize);
        }

        public int PlayerStorageLimit { get => storagePlayer.StorageLimit; }

        private int equippedItemStorageIndex = -1;
        private Storage equippedItemStorage = null;
        public bool ItemEquipped { get => equippedItemStorageIndex != -1; }

        // Returns the equipped items ID
        public int EquippedUI_ID { 
            get 
            { 
                if(equippedItemStorage == null)
                {
                    return -1;
                }
                if(equippedItemStorage == storageHotbar)
                {
                    return hotbarUiElementIdList[equippedItemStorageIndex];
                }
                else
                {
                    return inventoryUiElementIdList[equippedItemStorageIndex];
                }
            } 
        }


        public string EquippedItemId { get => equippedItemStorage.GetItemData(equippedItemStorageIndex).ID; }

        // Sets the selected items ID
        public void SetSelectedItemTo(int ui_id)
        {
            selectedItemUIID = ui_id;
        }

        // Sets selected item to -1, so its reseted 
        public void ResetSelectedItem()
        {
            selectedItemUIID = -1;
        }   

        // Adds a new hotbar element to the list
        public void AddHotbarUiElement(int ui_id)
        {
            hotbarUiElementIdList.Add(ui_id);
        }
        
        // Adds a new inventory element to the list
        public void AddInventoryUiElement(int ui_id)
        {
            inventoryUiElementIdList.Add(ui_id);
        }

        // Clears the inventory list
        public void ClearInventoryUIElements()
        {
            inventoryUiElementIdList.Clear();
        }

        // Adds the givet item to the storage
        public int AddToStorage(IInventoryItem item)
        {
            int countLeft = item.Count;
            if (storageHotbar.CheckIfStorageContains(item.ID))
            {
                countLeft = storageHotbar.AddItem(item);
                updateHotbarCallback.Invoke();
                if (countLeft == 0)
                {
                    return countLeft;
                }
            }
            countLeft = storagePlayer.AddItem(item.ID, countLeft, item.IsStackable, item.StackLimit);
            if(countLeft > 0)
            {
                countLeft = storageHotbar.AddItem(item.ID, countLeft, item.IsStackable, item.StackLimit);
                updateHotbarCallback.Invoke();
                if (countLeft == 0)
                {
                    return countLeft;
                }
            }
            return countLeft;
        }

        // Take a given amount from the item (for the crafting)
        internal int TakeFromItem(string ID, int count)
        {
            int tempCount = 0;
            tempCount += TakeFromStorage(storageHotbar, ID, count);
            if (tempCount == count)
            {
                return count;
            }
            else
            {
                tempCount += TakeFromStorage(storagePlayer, ID, count);
            }
            return tempCount;
        }

        // The a certain amount from the storage (we need to take away from the storage as weel not just from the UI)
        private int TakeFromStorage(Storage storage, string iD, int count)
        {
            var tempQuantity = storage.GetItemCount(iD);
            if(tempQuantity > 0)
            {
                if(tempQuantity >= count)
                {
                    storage.TakeItemFromStorageIfContaintEnough(iD, count);
                    return count;
                }
                else
                {
                    storage.TakeItemFromStorageIfContaintEnough(iD, tempQuantity);
                    return tempQuantity;
                }
            }
            return 0;
        }

        // Checks if the item is full or not (returns true if we have space false if its full)
        internal bool CheckItemInStorage(string id, int count)
        {
            int quantity = storagePlayer.GetItemCount(id);
            quantity += storageHotbar.GetItemCount(id);
            if(quantity >= count)
            {
                return true;
            }
            return false;
        }

        //Checks if the item is full or not in the storage hotbar (not in the inventory)
        internal bool CheckIfStorageIsFull()
        {
            return storageHotbar.CheckIfStorageIsFull() && storagePlayer.CheckIfStorageIsFull();
        }

        // Checks if we clicked to an empty spot or to an item
        internal bool CheckIfSelectedItemIsEmpty(int ui_id)
        {
            if (CheckItemForHotbarStorageNotEmpty(ui_id))
            {
                return storageHotbar.CheckIfItemIsEmpty(hotbarUiElementIdList.IndexOf(ui_id));
            }
            else if (CheckItemForInventoryStorageNotEmpty(ui_id))
            {
                return storagePlayer.CheckIfItemIsEmpty(inventoryUiElementIdList.IndexOf(ui_id));
            }
            else
            {
                return true;
            }
        }

        // Unequipping the item (-1 value)
        internal void UnequipItem()
        {
            if(equippedItemStorageIndex != -1)
            {
                equippedItemStorageIndex = -1;
                equippedItemStorage = null;
            }
        }

        // equpping the item that was given by the id (if its inventory or hotbar)
        internal void EquipItem(int ui_id)
        {
            UnequipItem();
            if (hotbarUiElementIdList.Contains(ui_id))
            {
                equippedItemStorageIndex = hotbarUiElementIdList.IndexOf(ui_id);
                equippedItemStorage = storageHotbar;
            }
            else if (inventoryUiElementIdList.Contains(ui_id) && storagePlayer.CheckIfItemIsEmpty(inventoryUiElementIdList.IndexOf(ui_id))==false)
            {
                equippedItemStorageIndex = inventoryUiElementIdList.IndexOf(ui_id);
                equippedItemStorage = storagePlayer;
            }
            else
            {
                throw new Exception("No item with ui_id " + ui_id);
            }
        }

        // Taking away an item if its needed (for example for the crafting system)
        internal void TakeOneFromItem(int ui_id)
        {
            if (CheckItemForHotbarStorageNotEmpty(ui_id))
            {
                storageHotbar.TakeFromItemWith(hotbarUiElementIdList.IndexOf(ui_id),1);
                updateHotbarCallback();
            }
            else if (CheckItemForInventoryStorageNotEmpty(ui_id))
            {
                storagePlayer.TakeFromItemWith(inventoryUiElementIdList.IndexOf(ui_id), 1);
            }
            else
            {
                throw new Exception("No item with ui id " + ui_id);
            }
        }

        // Returns the items count (hotbar or inventory or if its 0)
        internal int GetItemCountFor(int ui_id)
        {
            if (CheckItemForHotbarStorageNotEmpty(ui_id))
            {
                return storageHotbar.GetCountOfItemWithIndex(hotbarUiElementIdList.IndexOf(ui_id));
            }
            else if (CheckItemForInventoryStorageNotEmpty(ui_id))
            {
                return storagePlayer.GetCountOfItemWithIndex(inventoryUiElementIdList.IndexOf(ui_id));
            }
            else
            {
                return -1;
            }
        }

        // Removes the item from the inventory
        internal void RemoveItemFromInventory(int ui_id)
        {
            if (hotbarUiElementIdList.Contains(ui_id))
            {
                storageHotbar.RemoveItemOfIndex(hotbarUiElementIdList.IndexOf(ui_id));
            }
            else if (inventoryUiElementIdList.Contains(ui_id))
            {
                storagePlayer.RemoveItemOfIndex(inventoryUiElementIdList.IndexOf(ui_id));
            }
            else
            {
                throw new Exception("No item with id " + ui_id);
            }
            ResetSelectedItem();

        }

        // Returns the hotbar items
        public List<ItemData> GetItemsDataForHotbar()
        {
            return storageHotbar.GetItemsData();
        }
        
        // Returns the hotbar items
        public List<ItemData> GetItemsDataForInventory()
        {
            return storagePlayer.GetItemsData();
        }

        // We have to swap the items as we did it and I explaind in the inventory. But that was just for the UI and not
        // the data (which we can think of a 'backend' or like a database but in the game, continously updating and changing)
        
        // Swapping inside the inventory (between the droppend and the dragged item)
        internal void SwapStorageItemsInsideInventory(int droppedItemID, int draggedItemID)
        {
            var storage_IdDraggedItem = inventoryUiElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storagePlayer.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = inventoryUiElementIdList.IndexOf(droppedItemID);

            // We need to check if its empty so we can place it or swap it.

            // If there is an item placed we spawn it
            if (CheckItemForInventoryStorageNotEmpty(droppedItemID))
            {
                
                var storagedata_IdDroppedItem = storagePlayer.GetItemData(storage_IdDroppedItem);

                storagePlayer.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            
            // If not we just place it
            else
            {   

                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storagePlayer.RemoveItemOfIndex(storage_IdDraggedItem);
            }

        }

        // Checking if the current place is empty or not
        private bool CheckItemForInventoryStorageNotEmpty(int ui_id)
        {
            return inventoryUiElementIdList.Contains(ui_id) && storagePlayer.CheckIfItemIsEmpty(inventoryUiElementIdList.IndexOf(ui_id)) == false;
        }   

        // From here this is the same system
        internal void SwapStorageItemsInsideHotbar(int droppedItemID, int draggedItemID)
        {
            var storage_IdDraggedItem = hotbarUiElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storageHotbar.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = hotbarUiElementIdList.IndexOf(droppedItemID);

            if (CheckItemForHotbarStorageNotEmpty(droppedItemID))
            {

                var storagedata_IdDroppedItem = storageHotbar.GetItemData(storage_IdDroppedItem);

                storageHotbar.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else
            {
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storageHotbar.RemoveItemOfIndex(storage_IdDraggedItem);
            }

        }

        private bool CheckItemForHotbarStorageNotEmpty(int ui_id)
        {
            return hotbarUiElementIdList.Contains(ui_id) && storageHotbar.CheckIfItemIsEmpty(hotbarUiElementIdList.IndexOf(ui_id)) == false;
        }

        internal void SwapStorageHotbarToInventory(int droppedItemID, int draggedItemID)
        {
            var storage_IdDraggedItem = hotbarUiElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storageHotbar.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = inventoryUiElementIdList.IndexOf(droppedItemID);

            if (CheckItemForInventoryStorageNotEmpty(droppedItemID))
            {

                var storagedata_IdDroppedItem = storagePlayer.GetItemData(storage_IdDroppedItem);

                storageHotbar.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else
            {
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storageHotbar.RemoveItemOfIndex(storage_IdDraggedItem);
            }
        }

        internal void SwapStorageInventoryToHotbar(int droppedItemID, int draggedItemID)
        {
            var storage_IdDraggedItem = inventoryUiElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storagePlayer.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = hotbarUiElementIdList.IndexOf(droppedItemID);

            if (CheckItemForHotbarStorageNotEmpty(droppedItemID))
            {

                var storagedata_IdDroppedItem = storageHotbar.GetItemData(storage_IdDroppedItem);

                storagePlayer.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else
            {
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storagePlayer.RemoveItemOfIndex(storage_IdDraggedItem);
            }
        }// Till here


        // Return with teh ID of the item (hotbar and inventory )
        internal string GetItemIdFor(int ui_id)
        {
            if (CheckItemForInventoryStorageNotEmpty(ui_id))
            {
                return storagePlayer.GetIdOfItemWithIndex(inventoryUiElementIdList.IndexOf(ui_id));
            }else if (CheckItemForHotbarStorageNotEmpty(ui_id))
            {
                return storageHotbar.GetIdOfItemWithIndex(hotbarUiElementIdList.IndexOf(ui_id));
            }
            else
            {
                return null;
            }
        }

        // Saving data from the inventory ( same system as in the SaveSystem  explained)
        public SavedItemSystemData GetDataToSave()
        {
            // Create new struck
            return new SavedItemSystemData
            {
                // Specify the data to be saved
                playerStorageItems = storagePlayer.GetDataToSave(),
                hotbarStorageItems = storageHotbar.GetDataToSave(),
                playerStorageSize = storagePlayer.StorageLimit
            };
        }
        // Loading data from the json file (same system as in the SaveSystem  explained)
        public void LoadData(SavedItemSystemData dataToLoad)
        {
            storagePlayer = new Storage(dataToLoad.playerStorageSize);
            storageHotbar.ClearStorage();                                                                                                                                                                                            
            foreach (var item in dataToLoad.playerStorageItems)
            {
                if (item.IsNull == false)
                {
                    storagePlayer.SwapItemWithIndexFor(item.StorageIndex, item);
                }
            }
            foreach (var item in dataToLoad.hotbarStorageItems)
            {
                if (item.IsNull == false)
                {
                    storageHotbar.SwapItemWithIndexFor(item.StorageIndex, item);
                }
            }
            updateHotbarCallback.Invoke();
        }
    }

    // Data backup, loading
    [Serializable]
    public struct InventorySaveData
    {
        public List<ItemData> playerStorageItems, hotbarStorageItems;
        public int playerStorageSize;
    }

    [Serializable]
    public struct SavedItemSystemData
    {
        // playerstorage item storage list (inventory)
        public List<ItemData> playerStorageItems;
        //  hotbar item storage list (hotbar)
        public List<ItemData> hotbarStorageItems;
        public int playerStorageSize;
    }
}



