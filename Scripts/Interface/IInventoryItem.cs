using System;


namespace SVS.InventorySystem
{
    /// Interface providing a basic outline for the Data stored in the ItemStorage.
    public interface IInventoryItem 
    {
        string ID { get;}
        int Count { get;}
        bool IsStackable { get;}
        int StackLimit { get;}

        

    }
}