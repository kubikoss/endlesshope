using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    public GameObject craftingPanel;
    public InventorySlot outputSlot;
    public GameObject inventoryItemPrefab;

    public List<InventorySlot> craftingSlots;
    public List<CraftingRecipes> allRecipes;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        craftingPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            OpenCraftingMenu();
        }
        CheckCraftingSlotsForRecipes();
    }

    private void CheckCraftingSlotsForRecipes()
    {
        string recipe = GetCraftingPattern();
        bool didCraft = false;
        if(!string.IsNullOrEmpty(recipe))
        {
            foreach(CraftingRecipes craftingRecipe in allRecipes)
            {
                if(recipe == GetPatternFromRecipes(craftingRecipe) && didCraft == false)
                {
                    Debug.Log("ok");
                    SpawnCraftingItem(craftingRecipe.resultItem);
                    didCraft = true;
                    return;
                }
                else
                {
                    Debug.Log("nigga");
                    didCraft = false;
                }
            }
        }
        else
        {
            ClearOutputSlot();
        }
    }

    private void SpawnCraftingItem(Item item)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab);
        InventoryItem outputItem = newItem.GetComponent<InventoryItem>();
        outputItem.DisplayItemInInventory(item);
        Debug.Log(outputItem.item);

        outputItem.transform.SetParent(outputSlot.transform);
        outputItem.transform.position = outputSlot.transform.position;
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

    /*private void ClearCraftingSlots()
    {
        foreach(InventorySlot slot in craftingSlots)
        {
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            Destroy(item.gameObject);
        }
    }*/

    private void ClearOutputSlot()
    {
        foreach (Transform child in outputSlot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void OpenCraftingMenu()
    {
        bool isCraftingPanelOpened = !craftingPanel.activeSelf;
        craftingPanel.SetActive(isCraftingPanelOpened);
        InventoryManager.Instance.fullInventory.SetActive(isCraftingPanelOpened);

        Cursor.lockState = isCraftingPanelOpened ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isCraftingPanelOpened;

        InventoryManager.Instance.playerCam.enabled = !isCraftingPanelOpened;
    }
}
