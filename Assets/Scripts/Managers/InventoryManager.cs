using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<Item> equippableItems = new List<Item>(9);
    public List<Item> fullInventory = new List<Item>();

    public Hands hands;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddItem(Item item)
    {
        if(fullInventory.Count < 20)
        {
            fullInventory.Add(item);

            if (item is Hands)
            {
                if (!equippableItems.Contains(item))
                {
                    equippableItems.Insert(0, item);
                }
            }
            else if (equippableItems.Count < 9)
            {
                equippableItems.Add(item);
            }
        }
    }
    
    public void RemoveItem(Item item)
    {
        fullInventory.Remove(item);
        if (equippableItems.Contains(item))
        {
            equippableItems.Remove(item);
        }
    }

    public void EquipItemFromInventory(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < equippableItems.Count)
        {
            Item currentItem = equippableItems[slotIndex];
            PlayerAttack.Instance.EquipItem(currentItem);
        }
    }

    public void EquipFirstSlot()
    {
        for (int i = 0; i < equippableItems.Count; i++)
        {
            if (equippableItems[i] != null && !(equippableItems[i] is Hands))
            {
                PlayerAttack.Instance.EquipItem(equippableItems[i]);
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
    }
}