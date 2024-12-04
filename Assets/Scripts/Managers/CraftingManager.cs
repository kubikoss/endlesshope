using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    [Header("Crafting")]
    public List<InventorySlot> craftingSlots;
    public List<CraftingRecipes> allRecipes;
    public GameObject inventoryItemPrefab;

    [Header("Output")]
    public InventorySlot outputSlot;
    public InventoryItem outputItem;
    
    [Header("Crafting Panel")]
    public GameObject craftingPanel;
    public bool isCraftingPanelOpened;

    public bool isCrafted;

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
    }

    public void SpawnItem(InventorySlot invSlot)
    {
        if (invSlot != null)
        {
            isCrafted = true;
            InventoryItem invItem = invSlot.GetComponentInChildren<InventoryItem>();
            GameObject itemInWorld = Instantiate(invItem.item.ItemWorld, PlayerManager.Instance.player.transform.position, Quaternion.identity);
            itemInWorld.GetComponent<ItemPickup>().Interact();
            ClearCraftingSlots();
            ClearOutputSlot();
            Debug.Log(invItem.item.name);
            isCrafted = false;
        }
    }

    public void CheckCraftingSlotsForRecipes()
    {
        string recipe = GetCraftingPattern();
        if(!string.IsNullOrEmpty(recipe))
        {
            foreach(CraftingRecipes craftingRecipe in allRecipes)
            {
                if(recipe == GetPatternFromRecipes(craftingRecipe))
                {
                    SpawnCraftingItem(craftingRecipe.resultItem);
                    return;
                }
                else
                {
                    ClearOutputSlot();
                }
            }
        }
    }

    private void SpawnCraftingItem(Item item)
    {
        if(outputSlot.transform.childCount == 0)
        {
            GameObject inventoryObject = Instantiate(inventoryItemPrefab);
            outputItem = inventoryObject.GetComponent<InventoryItem>();
            outputItem.DisplayItemInInventory(item, true);
            Debug.Log(outputItem.item);

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

    public void OpenCraftingMenu()
    {
        isCraftingPanelOpened = !craftingPanel.activeSelf;
        craftingPanel.SetActive(isCraftingPanelOpened);
        InventoryManager.Instance.fullInventory.SetActive(isCraftingPanelOpened);

        Cursor.lockState = isCraftingPanelOpened ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isCraftingPanelOpened;

        InventoryManager.Instance.playerCam.enabled = !isCraftingPanelOpened;
    }
}
//TODO
// inventory item count > 1 -> can craft
// inventory & crafting panel opening fix