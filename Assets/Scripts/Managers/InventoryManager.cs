using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<InventorySlot> inventorySlots;

    public GameObject inventoryItemPrefab;
    public GameObject fullInventory;
    public PlayerCam playerCam;
    public Hands hands;

    public bool isInventoryOpened { get; private set; }
    public int maxStackedItems = 5;
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
        OpenInventory();
    }

    public bool AddItem(Item item)
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
                    return true;
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
                return true;
            }
        }
        return false;
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
                itemToDrop.transform.localPosition = new Vector3(0, 0, 0);
                RemoveItem(itemToDrop);
                itemToDrop.gameObject.SetActive(true);


                return itemToDrop;
            }
        }
        return null;
    }

    public void EquipItemFromInventory(int slotIndex)
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

    public void SpawnToInventory(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.DisplayItemInInventory(item);

        int invSlot = inventorySlots.IndexOf(slot);
        ChangeSelectedSlot(invSlot);
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

    public int GetHotbarCount()
    {
        int count = 0;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].GetComponentInChildren<InventoryItem>() != null)
            {
                count++;
            }
        }
        return count;
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

    public void ChangeSelectedSlot(int newSlot)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newSlot].Select();
        selectedSlot = newSlot;
    }

    /*public InventoryItem GetInventoryItem(Item item)
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
    }*/
}