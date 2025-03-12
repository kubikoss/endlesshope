using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    [Header("Crafting")]
    public List<InventorySlot> craftingSlots;
    public List<CraftingRecipes> allRecipes;
    public GameObject inventoryItemPrefab;
    public bool extractorPresent = false;
    public bool hasCollectedVenom = false;

    [Header("Output")]
    public InventorySlot outputSlot;
    public InventoryItem outputItem;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SpawnItem(InventorySlot invSlot)
    {
        if (invSlot != null)
        {
            InventoryItem invItem = invSlot.GetComponentInChildren<InventoryItem>();
            if (invItem != null)
            {
                Item itemToWorld = InventoryManager.Instance.InstantiateItem(false, true, invItem.item);
                ClearCraftingSlots();
                ClearOutputSlot();
            }
        }
    }

    public void CheckCraftingSlotsForRecipes()
    {
        extractorPresent = false;

        // Check for Extractor and venom
        foreach (InventorySlot slot in craftingSlots)
        {
            if (slot != null)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null)
                {
                    Extractor extractor = itemInSlot.item.GetComponent<Extractor>();
                    if (extractor != null)
                    {
                        extractorPresent = true;
                        if (extractor.collectedVenom)
                        {
                            hasCollectedVenom = true;
                        }
                        else
                        {
                            hasCollectedVenom = false;
                        }
                    }
                }
            }
        }

        if (extractorPresent && !hasCollectedVenom)
        {
            ClearOutputSlot();
            return;
        }

        // Item count check
        foreach (InventorySlot slot in craftingSlots)
        {
            if (slot != null)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null && itemInSlot.count > 1)
                {
                    ClearOutputSlot();
                    return;
                }
            }
        }

        // Recipe pattern check
        string recipeCheck = GetCraftingPattern();
        if (!string.IsNullOrEmpty(recipeCheck))
        {
            foreach (CraftingRecipes craftingRecipe in allRecipes)
            {
                if (recipeCheck == GetPatternFromRecipes(craftingRecipe))
                {
                    SpawnCraftingItem(craftingRecipe.resultItem);
                    return;
                }
            }
        }
        ClearOutputSlot();
    }

    private void SpawnCraftingItem(Item item)
    {
        if(outputSlot.transform.childCount == 0)
        {
            GameObject inventoryObject = Instantiate(inventoryItemPrefab);
            outputItem = inventoryObject.GetComponent<InventoryItem>();
            outputItem.DisplayItemInInventory(item, true);

            outputItem.transform.SetParent(outputSlot.transform);
            outputItem.transform.position = outputSlot.transform.position;
        }
    }

    private string GetCraftingPattern()
    {
        string pattern = "";
        foreach(InventorySlot slot in craftingSlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null)
            {
                pattern += itemInSlot.item.name;
            }
            else
            {
                pattern += "null";
            }
            pattern += " ";
        }
        return pattern.Trim();
    }

    private string GetPatternFromRecipes(CraftingRecipes recipe)
    {
        string pattern = "";
        foreach(Item item in recipe.requiredItems)
        {
            if(item != null)
            {
                pattern += item.name;
            }
            else
            {
                pattern += "null";
            }
            pattern += " ";
        }
        return pattern.Trim();
    }

    private void ClearCraftingSlots()
    {
        foreach(InventorySlot slot in craftingSlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);
                Destroy(itemInSlot.item.gameObject);
            }
        }
    }

    private void ClearOutputSlot()
    {
        foreach (Transform child in outputSlot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void TryPutItemsBackInInventory()
    {
        foreach (InventorySlot slot in craftingSlots)
        {
            if (slot.transform.childCount == 0) 
                continue;

            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                int currentStackCount = itemInSlot.count;
                for(int i = 0; i < currentStackCount; i++)
                {
                    Item itemToInventory = InventoryManager.Instance.InstantiateItem(false, true, itemInSlot.item);
                }
                Destroy(itemInSlot.gameObject);
                Destroy(itemInSlot.item.gameObject);
            }
        }
    }
}