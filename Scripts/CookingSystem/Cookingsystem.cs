using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookingsystem : MonoBehaviour
{
    public Action<RecipeSO> onCraftItemRequest;
    public Func<string, int, bool> onCheckResourceAvailability;
    public Func<bool> onCheckInventoryFull;

    public List<RecipeSO> craftingRecipes;
    List<int> recipeUiIdList = new List<int>();

    public UI_Cooking uiCooking;

    int currentRecipeUiId = -1;

    private void Start()
    {
        uiCooking = GetComponent<UI_Cooking>();
        uiCooking.onRecipeClicked += RecipeClickedHandler;
        uiCooking.onCraftButtonClicked += CraftRecipeHandler;
        uiCooking.BlockCraftButton();
    }

    // Toggles the crafting panel
    public void ToggleCraftingUI(bool saveLastViewedRecipe = false)
    {
        uiCooking.ToggleUI();
        // Load the recieptes
        if (saveLastViewedRecipe == false)
        {
            currentRecipeUiId = -1;
        }
        if(currentRecipeUiId!= -1)
        {
            RecheckIngredients();
        }
        // if the crafting ui is visible then we need the reciepe ui to be prepared
        if (uiCooking.Visible)
        {
            recipeUiIdList = uiCooking.PrepareRecipeItems(craftingRecipes);
        }
        
    }

    // Recheck the ingredients while we have reciepe
    public void RecheckIngredients()
    {
        if(currentRecipeUiId != -1)
        {
            RecipeClickedHandler(currentRecipeUiId);
        }
    }

    // Handler for the Reciepe panel. Invoke the needed reciepe (by its index)
    private void CraftRecipeHandler()
    {
        var recipeIndex = recipeUiIdList.IndexOf(currentRecipeUiId);
        var recipe = craftingRecipes[recipeIndex];
        onCraftItemRequest.Invoke(recipe);
    }

    // Important method of the system. Responsible for the crafting
    private void RecipeClickedHandler(int id)
    {
        // We crafting the item

        // The recipeUIID wull be the id we work with
        currentRecipeUiId = id;
        // We clear the ingredients first
        uiCooking.ClearIngredients();
        // Gives us the recipe
        var recipeIndex = recipeUiIdList.IndexOf(currentRecipeUiId);
        var recipe = craftingRecipes[recipeIndex];
        var ingredientsIdCountDict = recipe.GetIngredientsIdValueDict();

        // Enables to click the button
        bool blockCraftButton = false;
        // Go through the ingredients
        foreach (var key in ingredientsIdCountDict.Keys)
        {
            // While there is enough number of the required item, we dont block the craft button, so in the end if everything is in the inventory we can craft the item
            bool enoughItemFlag = onCheckResourceAvailability.Invoke(key, ingredientsIdCountDict[key]);
            if(blockCraftButton == false)
            {
                blockCraftButton = !enoughItemFlag;
            }
            // Add the required ingredient
            uiCooking.AddIngredient(ItemDataManager.instance.GetItemName(key), ItemDataManager.instance.GetItemSprite(key), ingredientsIdCountDict[key], enoughItemFlag); 
        }

        // After that we can now show the ingredients panel (because we know that if we can craft the selected item or not)
        uiCooking.ShowIngredientsUI();
        // Block the craft button if there is not enough number of required item (or no required item) 
        if (blockCraftButton)
        {
            uiCooking.BlockCraftButton();
        }
        // Enable to craft 
        else
        {
            uiCooking.UnblockCraftButton();
        }
        // But if the inventory is full then we cant craft. We Show the red Text that says 'the inventory is full'
        if (onCheckInventoryFull.Invoke())
        {
            uiCooking.ShowInventoryFull();
        }
    }
}
