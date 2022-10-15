using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager instance;
    public Transform playerTransform;
    public string pickableLayerMask;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    internal PlacementHelper CreateStructure(StructureItemSO structureData)
    {
        var structure = Instantiate(structureData.GetPrefab(), playerTransform.position + playerTransform.forward + playerTransform.forward, Quaternion.identity);
        var collider = structure.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        var rb = structure.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        var placementHelper = structure.AddComponent<PlacementHelper>();
        placementHelper.Initialize(playerTransform);
        return placementHelper;
    }



    private void SpawnItems(Transform itemSpawnerTransform, ItemSpawner spawner)
    {
        Vector3 randomPosition = GenerateRandomPosition(spawner.radius);

        if (spawner.singleObject && spawner.itemToSpawn.isStackable)
        {
            CreateItemInPlace(itemSpawnerTransform.position + randomPosition, spawner.itemToSpawn, spawner.count);
        }
        else
        {
            for (int i = 0; i < spawner.count; i++)
            {
                CreateItemInPlace(itemSpawnerTransform.position + randomPosition, spawner.itemToSpawn, 1);
                randomPosition = GenerateRandomPosition(spawner.radius);

            }
        }
    }

  
    internal void CreateItemInPlace(Vector3 hitpoint, MaterialSO itemToSpawn, int resourceCountToSpawn)
    {
        var itemGameObject = Instantiate(itemToSpawn.GetModel(), hitpoint + Vector3.up * 0.2f, Quaternion.identity);
        PrepareItemGameObject(itemToSpawn.ID, resourceCountToSpawn, itemGameObject);
    }
    
    //Creates the item. Spawns it. But not I dont spawn the items, because I cant save the spawned items right now, just if a I place it to random palces
    private void CreateItemInPlace(Vector3 randomPosition, ItemSO itemToSpawn, int count)
    {
        
    }

    // Random positon generator
    private Vector3 GenerateRandomPosition(float radius)
    {
        return new Vector3(Random.Range(-radius, radius), Random.Range(0, radius), Random.Range(-radius, radius));
    }

    // When I drop the item it drops in front of our feet
    public void CreateItemAtPlayersFeet(string itemID, int currentItemCount)
    {
        var itemPrefab = ItemDataManager.instance.GetItemPrefab(itemID);
        var itemGameObject = Instantiate(itemPrefab, playerTransform.position + Vector3.up, Quaternion.identity);
        PrepareItemGameObject(itemID, currentItemCount, itemGameObject);
    }

    // I have to create the item we drop as a prefab, so we can drop it and pick it back after
    private void PrepareItemGameObject(string itemID, int currentItemCount, GameObject itemGameObject)
    {   
        // I give it box collider
        itemGameObject.AddComponent<BoxCollider>();
        // Rigidbody
        var rb = itemGameObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        // Pickable item type
        var pickableItem = itemGameObject.AddComponent<PickableItem>();
        // The count of the item
        pickableItem.SetCount(currentItemCount);
        // The ID
        pickableItem.dataSource = ItemDataManager.instance.GetItemData(itemID);
        // And the layer mask(the pickable layer) so I can interract with it
        itemGameObject.layer = LayerMask.NameToLayer(pickableLayerMask);
    }

    // It doesnt work tight now, it removes from our hand when we drop it. But when we have a waepon or a tool which we can use,
    // then I cant simply drop the HandWith(Tool-Forexample Pickaxe). I can drop just an item. Thats why I dont use the drop right now,
    // Because it doesnt work for equipable items
    internal void RemoveItemFromPlayerHand()
    {
        foreach (Transform child in playerTransform.GetComponent<AgentController>().itemSlot)
        {
            Destroy(child.gameObject);
        }

    }

    //Cereates the object and equipes it
    internal void CreateItemObjectInPlayerHand(string itemID)
    {
        var itemPrefab = ItemDataManager.instance.GetItemPrefab(itemID);
        var item = Instantiate(itemPrefab, playerTransform.GetComponent<AgentController>().itemSlot);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }
}
