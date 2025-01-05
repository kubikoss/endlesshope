using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public BaseItem itemData;
    public string ItemName => itemData.name;
    public int ID => itemData.ID;
    public Sprite ItemIcon => itemData.itemIcon;
    public int MaxStackCount => itemData.maxStackCount;
    public bool IsStackable => itemData.isStackable;
    public GameObject ItemWorld => itemData.itemWorld;

    public void UpdateInventoryItemCountOnUse()
    {
        InventoryItem inventoryItem = InventoryManager.Instance.GetInventoryItem(this);
        if (inventoryItem != null)
        {
            inventoryItem.count--;
            inventoryItem.UpdateCount();
            if (inventoryItem.count < 1)
            {
                InventoryManager.Instance.hotbarCount--;
                inventoryItem.RemoveItemFromInventory();
                InventoryManager.Instance.EquipHands();
                Destroy(inventoryItem.gameObject);
                Destroy(gameObject);
            }
        }
    }
}