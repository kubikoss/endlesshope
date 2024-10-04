using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<Item> fullInventory = new List<Item>(20);


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
        }
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
            PlayerAttack.Instance.EquipItem(currentItem);
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
    }
}