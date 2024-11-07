using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Food : Item
{
    public FoodItem foodItemData;

    public void Eat()
    {
        Player.Instance.UpdateHunger(this);
        /*InventoryManager.Instance.RemoveItem(this);
        InventoryManager.Instance.EquipFirstSlot();*/

        //Destroy(gameObject);

        UpdateInventoryItemCountOnUse();
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