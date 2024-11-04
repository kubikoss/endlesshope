using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<InventorySlot> inventorySlots;
    public List<Item> items = new List<Item>();

    public GameObject inventoryItemPrefab;
    public GameObject fullInventory;
    public Hands hands;
    public PlayerCam playerCam;

    public bool isInventoryOpened { get; private set; }
    public int maxStackedItems = 5;
    public int hotbarCount = 0;
    int selectedSlot = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        AddItem(hands);
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        SwitchItem();
        OpenInventory();
    }

    #region add/remove from inventory
    public int AddItem(Item item)
    {
        // Stacking item
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item.ID == item.ID && itemInSlot.count < maxStackedItems)
            {
                if (itemInSlot.item.IsStackable && item.IsStackable)
                {
                    itemInSlot.count++;
                    itemInSlot.UpdateCount();
                    Destroy(item.gameObject);
                    return 2; // Item stacked
                }
            }
        }

        // Empty slot in hotbar
        for (int i = 0; i < 9; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                if (hotbarCount < 9) // Limit
                {
                    hotbarCount++;
                    SpawnToInventory(item, slot);
                    items.Add(item);
                    return 1; // Item equipped
                }
            }
        }

        // No empty slot in hotbar
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                SpawnToInventory(item, slot);
                items.Add(item);
                return 3; // Item put in inventory, but not equipped
            }
        }

        return -1; // Full inventory
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == itemToRemove)
            {
                itemInSlot.RemoveItemFromInventory();
                itemToRemove.transform.SetParent(null);
                items.Remove(itemToRemove);
                break;
            }
        }
    }

    public Item DropItem(Item itemToDrop)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == itemToDrop)
            {
                if (itemInSlot.count == 1)
                {
                    itemToDrop.transform.localPosition = new Vector3(0, 0, 0);
                    RemoveItem(itemToDrop);
                    itemToDrop.gameObject.SetActive(true);
                    hotbarCount--;
                    return itemToDrop;
                }
                else if (itemInSlot.count > 1)
                {
                    Item newItem = Instantiate(itemInSlot.item, new Vector3(0, 0, 0), Quaternion.identity);
                    newItem.transform.SetParent(null);
                    itemInSlot.count--;
                    itemInSlot.UpdateCount();
                    return newItem;
                }
            }
        }
        return null;
    }

    private void SpawnToInventory(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.DisplayItemInInventory(item);

        int invSlot = inventorySlots.IndexOf(slot);
        ChangeSelectedSlot(invSlot);
    }
    #endregion
    #region item equipping/switching
    private void SwitchItem()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && !isInventoryOpened)
            {
                EquipItemFromInventory(i);
                ChangeSelectedSlot(i);
            }
        }
    }

    private void EquipItemFromInventory(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventorySlots.Count)
        {
            InventorySlot slot = inventorySlots[slotIndex];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                PlayerAttack.Instance.EquipItem(itemInSlot.item);
                ChangeSelectedSlot(slotIndex);
            }
        }
    }

    public void EquipFirstSlot()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                PlayerAttack.Instance.EquipItem(itemInSlot.item);
                ChangeSelectedSlot(i);
                return;
            }
        }
        PlayerAttack.Instance.EquipItem(hands);
    }
    #endregion
    #region ui/get method
    public void ChangeSelectedSlot(int newSlot)
    {
        int hotbarSlots = 9;

        if (newSlot < 0 || newSlot >= hotbarSlots)
            return;

        if (selectedSlot >= 0 && selectedSlot < hotbarSlots)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        InventoryItem itemInSlot = inventorySlots[newSlot].GetComponentInChildren<InventoryItem>();
        if(itemInSlot == null)
        {
            PlayerAttack.Instance.EquipItem(hands);
        }
        inventorySlots[newSlot].Select();
        selectedSlot = newSlot;
    }

    private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpened = !isInventoryOpened;
            fullInventory.SetActive(isInventoryOpened);

            Cursor.lockState = isInventoryOpened ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isInventoryOpened;

            playerCam.enabled = !isInventoryOpened;
        }
    }

    public InventoryItem GetInventoryItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item)
            {
                return itemInSlot;
            }
        }
        return null;
    }
    #endregion
}

// TODO
// inventory rework (90%):
// moving item from hotbar - decrease count + current slot selected -> moved item = currentitem
// ~ stack system (70%) - drop position + player drop item method - current item fix + (grenade stack when throwing fix, food/healable use -> count-- in hotbar)
// crafting system (0%)