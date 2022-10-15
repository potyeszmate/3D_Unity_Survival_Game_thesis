using SVS.InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewRecipeData", menuName ="Crafting/Recipe")]
public class RecipeSO : ScriptableObject, IInventoryItem
{
    // az item neve
    public string recipeName;
    // Az item típusa amit craftolni lehet
    public ItemSO outcome;
    // Mennyit craftolhatunk 
    
    [Range(1,10)]
    public int outcomeQuantity = 1;
    // A hozzávaló itemek listája
    public List<RecipeIngredients> ingredientsRequired;

    // Az UIinventory interface implementálása után létrejött adattagokatnak értéket adunk (ezek az item adatai)
    public string ID => outcome.ID;

    public int Count => outcomeQuantity;

    public bool IsStackable => outcome.isStackable;

    public int StackLimit => outcome.stackLimit;

    
    public Dictionary<string, int> GetIngredientsIdValueDict()
    {
        Dictionary<string, int> ingredientsDict = new Dictionary<string, int>();
        foreach (var item in ingredientsRequired)
        {
            ingredientsDict.Add(item.ingredient.ID, item.count);
        }
        return ingredientsDict;
    }
    // Returns the sprite of the finished, crafted item
    public Sprite GEtOutcomeSprite()
    {
        return outcome.imageSprite;
    }
}

// The ingredients and the given quantity
[Serializable]
public struct RecipeIngredients
{
    public ItemSO ingredient;
    public int count;
}
