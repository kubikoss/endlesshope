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
                    ClearCraftingSlots();
                    return;
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
        GameObject inventoryObject = Instantiate(inventoryItemPrefab);
        InventoryItem outputItem = inventoryObject.GetComponent<InventoryItem>();
        outputItem.DisplayItemInInventory(item);
        Debug.Log(outputItem.item);
        
        GameObject blud = Instantiate(item.ItemWorld, PlayerAttack.Instance.transform.position, Quaternion.identity);
        Transform itemHolder = GameObject.Find("ItemHolder").transform;
        blud.transform.SetParent(itemHolder);
        blud.transform.localPosition = new Vector3(0.58f, -0.14f, 0.682f);
        InventoryManager.Instance.AddItem(item);

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

    private void OpenCraftingMenu()
    {
        bool isCraftingPanelOpened = !craftingPanel.activeSelf;
        craftingPanel.SetActive(isCraftingPanelOpened);
        InventoryManager.Instance.fullInventory.SetActive(isCraftingPanelOpened);
        PlayerAttack.Instance.GetComponent<PlayerAttack>().enabled = !isCraftingPanelOpened;

        Cursor.lockState = isCraftingPanelOpened ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isCraftingPanelOpened;

        InventoryManager.Instance.playerCam.enabled = !isCraftingPanelOpened;
    }
}
