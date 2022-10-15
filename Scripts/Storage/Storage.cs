using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SVS.InventorySystem
{   

    public class Storage
    {
        int storageLimit = 0;

        public int StorageLimit { get => storageLimit; private set => storageLimit = value; }

        List<StorageItem> storageItems;
        public Storage(int storageSize)
        {
            this.StorageLimit = storageSize;
            storageItems = new List<StorageItem>();
            for (int i = 0; i < this.StorageLimit; i++)
            {
                storageItems.Add(null);
            }
        }

        
        // Returns 0 if successfuly added.
        public int AddItem(IInventoryItem inventoryItem)
        {
            int remainingAmount = TryAddingToAnExistingItem(inventoryItem.ID, inventoryItem.Count);
            if (remainingAmount == 0 || CheckIfStorageIsFull())
            {
                return remainingAmount;
            }

            StorageItem newStorageItem = new StorageItem(inventoryItem.ID, remainingAmount, inventoryItem.IsStackable, inventoryItem.StackLimit);

            int nullItemIndex = storageItems.FindIndex(x => x == null);
            
            if (nullItemIndex == -1)
            {
                
                storageItems.Add(newStorageItem);
            }
            else
            {
                storageItems[nullItemIndex] = newStorageItem;
            }
            return 0;
        }

        
        // Returns 0 if successfuly added. Else return amount that is left.
        public int AddItem(string ID, int count, bool isStackable = true, int stackLimit = 100)
        {
            int remainingAmount = TryAddingToAnExistingItem(ID, count);
            if (remainingAmount == 0 || CheckIfStorageIsFull())
            {
                return remainingAmount;
            }
            StorageItem newStorageItem = new StorageItem(ID, remainingAmount, isStackable, stackLimit);

            int nullItemIndex = storageItems.FindIndex(x => x == null);

            if (nullItemIndex == -1)
            {

                storageItems.Add(newStorageItem);
            }
            else
            {
                storageItems[nullItemIndex] = newStorageItem;
            }

            return 0;
        }

        // Returns the count if an item 
        internal int GetItemCount(string ID)
        {
            int quantity = 0;
            foreach (var item in storageItems)
            {
                if (item == null)
                    continue;
                if (item.ID == ID)
                    quantity += item.Count;
            }
            return quantity;
        }

        
        /// Swaps item with Index to provided InventoryItem data
        public void SwapItemWithIndexFor(int index, IInventoryItem inventoryItemData)
        {
            storageItems[index] = null;
            StorageItem newStorageItem = new StorageItem(inventoryItemData.ID, inventoryItemData.Count, inventoryItemData.IsStackable, inventoryItemData.StackLimit);
            storageItems[index] = newStorageItem;
        }

        
    
        // Returns false if any element in the storage is null. To add to existing item use AddItem method.
        public bool CheckIfStorageIsFull()
        {
            return storageItems.Any(x => x == null)==false;
        }

        // This methcod checks if we can add an item to the same panel spot (so if we can stack it or is it full)
        private int TryAddingToAnExistingItem(string ID, int itemCount)
        {
            if (itemCount == 0)
            {
                return 0;
            }
            if (CheckIfStorageContains(ID))
            {
                foreach (var item in storageItems)
                {
                    if (item != null && item.ID == ID && item.IsFull == false)
                    {
                        itemCount = item.AddToItem(itemCount);
                    }
                    if (itemCount == 0)
                    {
                        return 0;
                    }
                }
            }
            return itemCount;
        }

        
        // Returns true if storage contains item with DataID  
        public bool CheckIfStorageContains(string ID)
        {
            return storageItems.Any(x => x != null && x.ID == ID);
        }

        
        // If storage contains enought of item with ID it will be subtracted from the storage
        // Returns true if item was taken
        public bool TakeItemFromStorageIfContaintEnough(string ID, int quantity)
        {
            if (CheckIfStorageHasEnoughOfItemWith(ID, quantity) == false)
            {
                return false;
            }
            for (int i = storageItems.Count - 1; i >= 0; i--)
            {
                if(storageItems[i] == null)
                {
                    continue;
                }
                if (storageItems[i].ID == ID)
                {
                    quantity -= storageItems[i].TakeFromItem(quantity);
                }
                if (storageItems[i].IsEmpty)
                {
                    storageItems[i] = null;
                }
                if (quantity <= 0)
                {
                    return true;
                }
            }
            throw new Exception("Something went wrong.");
        }

        
        // Take AmountToTake of items from item with Index.
        // Returns how much was taken
        public int TakeFromItemWith(int index, int amountToTake)
        {
            if(storageItems[index]==null )
            {
                return 0;
            }
            if(storageItems[index].Count < amountToTake)
            {
                storageItems[index] = null;
                return storageItems[index].Count;
            }
            storageItems[index].TakeFromItem(amountToTake);
            if (storageItems[index].IsEmpty)
            {
                storageItems[index] = null;
            }
            return amountToTake;
        }

        
        // Removes item with Index from storage without any checks.
        public void RemoveItemOfIndex(int index)
        {
            storageItems[index] = null;
        }

        
        // Check if storage contains AmountToTake of item with Index
        // Returns true if storage contains enough item<
        public bool CheckIfStorageHasEnoughOfItemWith(string index, int amountToTake)
        {
            int quantity = 0;
            foreach (var item in storageItems)
            {
                if (item == null)
                    continue;
                if (item.ID == index)
                {
                    quantity += item.Count;
                }
                if (quantity >= amountToTake)
                {
                    return true;
                }
            }
            return false;
        }

        
        // Increases the storage size;
        public void UpgradeStorage(int capacity)
        {
            StorageLimit += capacity;
            for (int i = 0; i < capacity; i++)
            {
                storageItems.Add(null);
            }
        }

        
        // Removes all the stoage items
        public void ClearStorage()
        {
            storageItems.Clear();
            for (int i = 0; i < this.StorageLimit; i++)
            {
                storageItems.Add(null);
            }
        }

        
        // Returns copy of data for all the storage items.
        // Returns the list of ItemData for all items inside storage
        public List<ItemData> GetItemsData()
        {
            List<ItemData> valueToReturn = new List<ItemData>();
            for (int i = 0; i < this.StorageLimit; i++)
            {
                if (storageItems[i] != null)
                {
                    valueToReturn.Add(new ItemData(i, storageItems[i].Count, storageItems[i].ID, storageItems[i].IsStackable, storageItems[i].StackLimit));
                }
                else
                {
                    valueToReturn.Add(new ItemData(true));
                }
            }

            return valueToReturn;
        }

        
        // Returns a data copy of item with Index
        // Returns the itemData for the item in the storage. Null if not present
        public ItemData GetItemData(int index)
        {
            if (storageItems[index] == null)
            {
                return new ItemData(true);
            }
            return new ItemData(index, storageItems[index].Count, storageItems[index].ID, storageItems[index].IsStackable, storageItems[index].StackLimit);
        }

        
        // Returns the ID of the data stored inside the storage item.
        // Returns the ID of the storad item. null if not present
        public string GetIdOfItemWithIndex(int index)
        {
            if (storageItems[index] == null)
            {
                return null;
            }
            return storageItems[index].ID;
        }

        
        // Returns the Count of the data stored inside the storage item.
        // Returns the count of the stored item. -1 if not present
        public int GetCountOfItemWithIndex(int index)
        {
            if (storageItems[index] == null)
            {
                return -1;
            }
            return storageItems[index].Count;
        }

        
        // Checks if item with Index is empty.
        // Returns true if item is null
        public bool CheckIfItemIsEmpty(int index)
        {
            return storageItems[index] == null;
        }

        
        // Gets index of the first element with null value
        // Returns an int as the index value
        public int GetIndexOfEmptyStorageSpace()
        {
            return storageItems.IndexOf(null);
        }

        // Returns with the item data
        public List<ItemData> GetDataToSave()
        {
            List<ItemData> valueToReturn = new List<ItemData>();
            for (int i = 0; i < this.StorageLimit; i++)
            {
                if (storageItems[i] != null)
                {
                    valueToReturn.Add(new ItemData(i, storageItems[i].Count, storageItems[i].ID, storageItems[i].IsStackable, storageItems[i].StackLimit));
                }
                else
                {
                    valueToReturn.Add(new ItemData(true));
                }
            }

            return valueToReturn;
        }

    }

    
    // Struct that will be used to pass the data from inventory as a copy
    [Serializable]
    public struct ItemData : IInventoryItem
    {
        [SerializeField]
        private bool isNull;
        [SerializeField]
        private int storageIndex;
        [SerializeField]
        private string id;
        [SerializeField]
        private int count;
        [SerializeField]
        private bool isStackable;
        [SerializeField]
        private int stackLimit;

    
        public bool IsNull
        {
            get { return isNull; }
            private set { isNull = value; }
        }

        public int StorageIndex
        {
            get { return storageIndex; }
            private set { storageIndex = value; }
        }

        public string ID
        {
            get { return id; }
            private set { id = value; }
        }

        public int Count
        {
            get { return count; }
            private set { count = value; }
        }

        public bool IsStackable
        {
            get { return isStackable; }
            private set { isStackable = value; }
        }

        public int StackLimit
        {
            get { return stackLimit; }
            private set { stackLimit = value; }
        }

        
        // Default non-null item constructor. The information all we need about the item (the items data)
        public ItemData(int storageIndex, int count, string id, bool isStackable, int stackLimit)
        {
            this.id = id;
            this.count = count;
            this.storageIndex = storageIndex;
            this.isStackable = isStackable;
            this.stackLimit = stackLimit;
            this.isNull = false;
        }

        // Default NUll item contructor. The same as above just for an empty item
        public ItemData(bool isNull = true)
        {
            this.id = "";
            this.count = -1;
            this.storageIndex = -1;
            this.isStackable = false;
            this.stackLimit = -1;
            this.isNull = true;
        }
    }
}