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
    public PlayerCam playerCam;

    public Hands hands;
    //public Item currentItem;

    public bool isInventoryOpened { get; private set; }
    public int maxStackedItems = 5;
    int selectedSlot = -1;
    int count = 0;

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

    public int AddItem(Item item)
    {
        //stacking item
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.ID == item.ID && itemInSlot.count < maxStackedItems)
            {
                if(itemInSlot.item.IsStackable && item.IsStackable)
                {
                    itemInSlot.count++;
                    itemInSlot.UpdateCount();
                    Destroy(item.gameObject);
                    return 2;
                }
            }
        }

        //empty slot
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnToInventory(item, slot);
                items.Add(item);
                return 1;
            }
        }
        return -1;
    }

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

    private void SpawnToInventory(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.DisplayItemInInventory(item);

        int invSlot = inventorySlots.IndexOf(slot);
        ChangeSelectedSlot(invSlot);
        /*if(GetHotbarCount() < 10)
        {
            ChangeSelectedSlot(invSlot);
        }
        else
        {
            ChangeSelectedSlot(8);
        }*/
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

    public void ChangeSelectedSlot(int newSlot)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
            if (inventorySlots[newSlot].GetComponentInChildren<InventoryItem>() == null)
            {
                PlayerAttack.Instance.EquipItem(hands);
            }
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

    public int GetHotbarCount()
    {
        count = 0;
        for (int i = 0; i < 10; i++)
        {
            if (inventorySlots[i].GetComponentInChildren<InventoryItem>() != null)
            {
                count++;
                Debug.Log(count);
            }
            if (count > 9)
            {
                return 10;
            }
        }
        return count;
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
}

// TODO
// inventory rework (80%):
// item pickup - check if can equip new item) + change selected slot to max 9th slot
// ~ stack system (70%, drop position + player drop item method - current item fix)
// crafting system (0%)