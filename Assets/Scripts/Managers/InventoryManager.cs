using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<Item> fullInventory = new List<Item>(20);
    /*public List<GameObject> inventorySlots = new List<GameObject>(20);
    public GameObject inventoryCanvas;*/

    public Hands hands;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (hands != null)
        {
            AddItem(hands);
        }
    }

    public void AddItem(Item item)
    {
        if(fullInventory.Count < 20)
        {
            fullInventory.Add(item);
        }
        /*bool equippableSlotsFull = false;

        for (int i = 0; i < 1; i++)
        {
            if (fullInventory[i] == null)
            {
                equippableSlotsFull = false;
                break;
            }
        }

        for (int i = 0; i < 1; i++)
        {
            if (fullInventory[i] == null)
            {
                fullInventory[i] = item;
                item.transform.SetParent(inventorySlots[i].transform);
                item.transform.localPosition = Vector3.zero;

                item.gameObject.SetActive(!equippableSlotsFull); 
                break;
            }
        }*/
    }

    public void RemoveItem(Item item)
    {
        fullInventory.Remove(item);
    }

    public void EquipItemFromInventory(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < fullInventory.Count)
        {
            Item currentItem = fullInventory[slotIndex];
            if (currentItem != null)
            {
                PlayerAttack.Instance.EquipItem(currentItem);
            }
        }
    }

    public void EquipFirstSlot()
    {
        for (int i = 0; i < fullInventory.Count; i++)
        {
            if (fullInventory[i] != null && !(fullInventory[i] is Hands))
            {
                PlayerAttack.Instance.EquipItem(fullInventory[i]);
                return;
            }
        }

        PlayerAttack.Instance.EquipItem(hands);
    }

    public void DropItem(Item itemToDrop)
    {
        RemoveItem(itemToDrop);
        itemToDrop.transform.SetParent(null);
        itemToDrop.gameObject.SetActive(true);
        itemToDrop.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
    }

    /*public void MoveItem(Item droppedItem, Item targetItem)
    {
        int droppedIndex = fullInventory.IndexOf(droppedItem);
        int targetIndex = fullInventory.IndexOf(targetItem);

        if (droppedIndex != -1 && targetIndex != -1)
        {
            fullInventory[droppedIndex] = targetItem;
            fullInventory[targetIndex] = droppedItem;

            droppedItem.transform.SetParent(inventorySlots[targetIndex].transform);
            targetItem.transform.SetParent(inventorySlots[droppedIndex].transform);
        }
    }*/
}