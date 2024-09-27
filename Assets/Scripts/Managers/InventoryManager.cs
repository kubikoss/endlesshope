using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<Item> equippableItems = new List<Item>(9);
    public List<Item> fullInventory = new List<Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
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

    public void EquipItemFromInventory(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < equippableItems.Count)
        {
            Item currentItem = equippableItems[slotIndex];
            PlayerAttack.Instance.EquipItem(currentItem);
        }
    }
}
