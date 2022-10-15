using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialHelper
{
    // This method swaps the materials
    public static void SwapToSelectionMaterial(GameObject objectToModify, List<Material[]> currentColliderMaterialsList, Material selectionMaterial)
    {
        currentColliderMaterialsList.Clear();
        PrepareRendererToSwapMaterials(objectToModify, currentColliderMaterialsList, selectionMaterial);
        if (objectToModify.transform.childCount > 0)
        {
            foreach (Transform child in objectToModify.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    PrepareRendererToSwapMaterials(child.gameObject, currentColliderMaterialsList, selectionMaterial);
                }
            }
        }
    }

    // Gives the renderer the material that is going to be swapped
    public static void PrepareRendererToSwapMaterials(GameObject objectToModify, List<Material[]> currentColliderMaterialsList, Material selectionMaterial)
    {
        var renderer = objectToModify.GetComponent<Renderer>();
        currentColliderMaterialsList.Add(renderer.sharedMaterials);
        SwapMaterials(renderer, selectionMaterial);
    }

    // Swaps the given material to the selection material 
    public static void SwapMaterials(Renderer renderer, Material selectionMaterial)
    {
        Material[] matArray = new Material[renderer.materials.Length];
        for (int i = 0; i < matArray.Length; i++)
        {
            matArray[i] = selectionMaterial;
        }
        renderer.materials = matArray;
    }

    // Swaps back to the original material from the selection material
    public static void SwapToOriginalMaterial(GameObject objectToModify, List<Material[]> currentColliderMaterialsList)
    {
        var renderer = objectToModify.GetComponent<Renderer>();
        renderer.materials = currentColliderMaterialsList[0];
        if (currentColliderMaterialsList.Count > 1)
        {
            for (int i = 0; i < currentColliderMaterialsList.Count; i++)
            {
                if (objectToModify.transform.GetChild(i).gameObject.activeSelf)
                {
                    var childRenderer = objectToModify.transform.GetChild(i).GetComponent<Renderer>();
                    childRenderer.materials = currentColliderMaterialsList[i];
                }
            }
        }
    }

    // This method enables the emission to the object ( this will be the material look when we try to pick up an item)
    public static void EnableEmission(GameObject gameObject, Color color)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            renderer.materials[i].EnableKeyword("_EMISSION");
            renderer.materials[i].SetColor("_EmissionColor", color);
        }
    }

    // Tgis method disables the emission in the object (when we dont pick up the item out of the items range)
    public static void DisableEmission(GameObject gameObject)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            renderer.materials[i].DisableKeyword("_EMISSION");
        }
    }
}
