using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class AgentController : MonoBehaviour
{
    public AgentMovement movement;
    public PlayerInput input;

    public InventorySystem inventorySystem;

    public CraftingSystem craftingSystem;

    public Cookingsystem cookingSystem;

    public FurnanceSystem furnanceSystem;

    public TradingSystem tradingSystem;

    public StructuresInteractionManager structeInteractionManager;

    public DetectionSystem detectionSystem;

    public GameManager gameManager;

    public Transform itemSlot;

    public AudioSource audioSource;

    public BuildingPlacementStorage buildingPlacementStroage;

    public Vector3? spawnPosition = null;

    public PlayerStat playerStat;

    BaseState currentState;
    public readonly BaseState movementState = new MovementState();
   
    public readonly BaseState inventoryState = new InventoryState();
    public readonly BaseState interactState = new InteractState();
    public readonly BaseState placementState = new PlacementState();


    private void OnEnable()
    {
        movement = GetComponent<AgentMovement>();
        input = GetComponent<PlayerInput>();
        currentState = movementState;
        currentState.EnterState(this);
        AssignInputListeners();
        detectionSystem = GetComponent<DetectionSystem>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        // Initialize

        spawnPosition = transform.position;
        inventorySystem.OnStructureUse += StructureUseCallback;
        craftingSystem.onCheckResourceAvailability += inventorySystem.CheckResourceAvailability;
        craftingSystem.onCheckInventoryFull += inventorySystem.CheckInventoryFull;
        craftingSystem.onCraftItemRequest += inventorySystem.CraftAnItem;
        inventorySystem.onInventoryStateChanged += craftingSystem.RecheckIngredients;

        ///CookingSystem
        cookingSystem.onCheckResourceAvailability += inventorySystem.CheckResourceAvailability;
        cookingSystem.onCheckInventoryFull += inventorySystem.CheckInventoryFull;
        cookingSystem.onCraftItemRequest += inventorySystem.CraftAnItem;
        inventorySystem.onInventoryStateChanged += cookingSystem.RecheckIngredients;

        // TradingSystem
        tradingSystem.onCheckResourceAvailability += inventorySystem.CheckResourceAvailability;
        tradingSystem.onCheckInventoryFull += inventorySystem.CheckInventoryFull;
        tradingSystem.onCraftItemRequest += inventorySystem.CraftAnItem;
        inventorySystem.onInventoryStateChanged += tradingSystem.RecheckIngredients;

        // FurnanceSystem
        furnanceSystem.onCheckResourceAvailability += inventorySystem.CheckResourceAvailability;
        furnanceSystem.onCheckInventoryFull += inventorySystem.CheckInventoryFull;
        furnanceSystem.onCraftItemRequest += inventorySystem.CraftAnItem;
        inventorySystem.onInventoryStateChanged += furnanceSystem.RecheckIngredients;

    }

    // Placing items and toggling the inventory to not show during the placement
    private void StructureUseCallback()
    {
        
        if (inventorySystem.InventoryVisible)
        {
            inventorySystem.ToggleInventory();
            craftingSystem.ToggleCraftingUI();
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
        }
        
        TransitionToState(placementState);
    }

    // Assigning the input listeners. Checking the input buttons and handleng them
    private void AssignInputListeners()
    {
        input.OnHotbarKey += HandleHotbarInput;
        input.OnToggleInventory += HandleInventoryInput;
        input.OnPrimaryAction += HandlePrimaryInput;
        input.OnSecondaryAction += HandleSecondaryInput;
        input.OnEscapeKey += HendleEscapeKey;
    }

    // Escape handler
    private void HendleEscapeKey()
    {
        currentState.HandleEscapeInput();
        
    }
    
    // Secondary input (right click) handler
    private void HandleSecondaryInput()
    {
        currentState.HandleSecondaryAction();
    }

    // Primary input (left click) handler
    private void HandlePrimaryInput()
    {
        currentState.HandlePrimaryAction();
    }

    // Inventory input (button i) handler
    private void HandleInventoryInput()
    {
        currentState.HandleInventoryInput();
    }

    // Hotbar inputs handler (1-8)
    private void HandleHotbarInput(int hotbarkey)
    {
        currentState.HandleHotbarInput(hotbarkey);
    }

    // Updating the current state
    private void Update()
    {
        if(Time.timeScale == 0)
            return;
        currentState.Update();
        
    }

    // Not using 
    private void OnDisable()
    {
        input.OnJump -= currentState.HandleJumpInput;
    }

    // Transitioning to these states
    public void TransitionToState(BaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    // Gizmo helper (show the gizmo we can reach the items)
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + input.MovementDirectionVector, detectionSystem.detectionRadius);
        }
    }

  

    // Sets the players spawn postition
    internal void SaveSpawnPoint()
    {
        spawnPosition = transform.position;
        
    }
    // Sets the players respawn position (after save file load)
    public void RespawnPlayer()
    {
        if (spawnPosition != null)
        {
            movement.TeleportPlayerTo(spawnPosition.Value + Vector3.up);
        }
    }



    // Equipping items if its a sing item type and not a item with hands (right now we just have items with the hand objects) 
    public void EquipToHand(GameObject model)
    {
        foreach (Transform child in itemSlot)
        {
            Destroy(child.gameObject);
        }

        var item = Instantiate(model, itemSlot);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

    }
}

