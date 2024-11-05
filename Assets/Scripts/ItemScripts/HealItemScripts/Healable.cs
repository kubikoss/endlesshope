using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healable : Item
{
    public int HealAmount => ((HealableItem)itemData).healAmount;

    public void Use()
    {
        HealPlayer();
    }

    public void HealPlayer()
    {
        Player.Instance.Heal(HealAmount);
        /*InventoryManager.Instance.RemoveItem(this);
        InventoryManager.Instance.EquipFirstSlot();

        Destroy(gameObject);*/

        UpdateInventoryItem();
    }

    /*private void UpdateInventoryItem()
    {
        InventoryItem inventoryItem = InventoryManager.Instance.GetInventoryItem(this);
        if (inventoryItem != null)
        {
            inventoryItem.count--;
            inventoryItem.UpdateCount();
            Debug.Log(inventoryItem.count);

            if (inventoryItem.count < 1)
            {
                InventoryManager.Instance.RemoveItem(this);
                InventoryManager.Instance.EquipFirstSlot();
                Destroy(inventoryItem.gameObject);
            }
        }
    }*/
}