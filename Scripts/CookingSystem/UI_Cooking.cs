using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Cooking : MonoBehaviour
{
    //Connects the data to the UI

    // Detection of the crafting button
    public Action onCraftButtonClicked;
    // Detection of the recipe button
    public Action<int> onRecipeClicked;
    // dictionary for itemrecipes
    private Dictionary<int, RecipeItemHelper> recipeUiElementDictionary = new Dictionary<int, RecipeItemHelper>();
    // Access all panels to display/hide them
    public GameObject craftingPanel, recipesItemPanel, ingredientsMainPanel, ingredientElementsPanel, inventoryFullText;
    // We also need prefabs
    public GameObject recipePrefab, ingredientPrefab,inventoryPanel;
    // Craft button that craft the item by clicking on it
    public Button craftBtn;
    // View the visibility of the crafting panel
    public bool Visible { get => craftingPanel.activeSelf; }

    private void Start()
    {
        //CookingHide();
        ClearUI();
        // Ingredients panel (not shown at first, only on click)
        ingredientsMainPanel.SetActive(false);
        // Crafting panel (appears when you are in inventory)
        craftingPanel.SetActive(false);
        // call craft button when pressed
        craftBtn.onClick.AddListener(() => onCraftButtonClicked.Invoke());
    }

    // Existing recipes should be deleted from the panel so that they do not appear more than once for each invitation
    private void ClearUI()
    {
        foreach (Transform child in recipesItemPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    // Displays the ingredients panel
    public void ShowIngredientsUI()
    {
        ingredientsMainPanel.SetActive(true);
    }
    // We should also delete the ingredients if they are already in the panel
    public void ClearIngredients()
    {
        foreach (Transform child in ingredientElementsPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    // This method is responsible for the UI display
    public void ToggleUI()
    {
        if(craftingPanel.activeSelf == false)
        {
            craftingPanel.SetActive(true);
            inventoryPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
    

        }
        else
        {
            Cursor.visible = false;
            ingredientsMainPanel.SetActive(false);
            inventoryPanel.SetActive(false);
            craftingPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    // Block the crafting button (if there are not enough ingredients)
    public void BlockCraftButton()
    {
        craftBtn.interactable = false;
    }

    // Enables the crafting button (if there are enough ingredients)
    public void UnblockCraftButton()
    {
        craftBtn.interactable = true;
    }
    // It detects when you click on a recipe (because it will then display its ingredients)
    private void OnRecipeClicked(int id)
    {
        if (recipeUiElementDictionary.ContainsKey(id))
        {
            onRecipeClicked.Invoke(id);
        }
    }
    // This method is needed here because if the inventory is full, then do not continue the function, i.e. do not add the item
    public void ShowInventoryFull()
    {
        var element = Instantiate(inventoryFullText, Vector3.zero, Quaternion.identity, ingredientElementsPanel.transform);
    }
    
    // Adds ingredients to the ingredients panel
    public void AddIngredient(string ingredientName, Sprite ingredientSprite, int ingredientCOunt, bool enoughItems)
    {
        var element = Instantiate(ingredientPrefab, Vector3.zero, Quaternion.identity, ingredientElementsPanel.transform);
        var ingredientHelper = element.GetComponent<IngredientItemElement>();
        ingredientHelper.SetItemUI(ingredientName, ingredientSprite, ingredientCOunt, enoughItems);
    }
    // Adds recipes to the recipe panel. It's more complicated because you have to enter your recipes and manage whether you clicked on the panel, etc.
    public List<int> PrepareRecipeItems(List<RecipeSO> listOfRecipes)
    {
        ClearUI();
        recipeUiElementDictionary.Clear();
        // recipe list
        List<int> recipeUiIdList = new List<int>();
        // create recipes
        foreach (var item in listOfRecipes)
        {
            var element = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity, recipesItemPanel.transform);
            var recipeHelper = element.GetComponent<RecipeItemHelper>();
            recipeHelper.OnClickEvent += OnRecipeClicked;
            recipeUiElementDictionary.Add(element.GetInstanceID(), recipeHelper);
            // The item details are set
            recipeHelper.SetItemUI(item.recipeName, item.GEtOutcomeSprite());

            recipeUiIdList.Add(element.GetInstanceID());
        }
        // Returns with a list of recipes (id is the identifier)
        return recipeUiIdList;
    }

    internal void CookingShow()
    {
        craftingPanel.SetActive(true);
    }

    internal void CookingHide()
    {
        craftingPanel.SetActive(false);
    }

   
}
