using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item; 

    public void Interact()
    {
        if (!InventoryManager.Instance.isInventoryOpened)
        {
            if (item != null)
            {
                int canAdd = InventoryManager.Instance.AddItem(item);

                if (canAdd > 0)
                {
                    Transform itemHolder = GameObject.Find("ItemHolder").transform;
                    item.transform.SetParent(itemHolder);
                    item.transform.localPosition = new Vector3(0.58f, -0.14f, 0.682f); 

                    if (canAdd == 1) 
                    {
                        PlayerAttack.Instance.EquipItem(item);
                        
                    }
                    else if (canAdd == 2) 
                    {
                        Item stackedItem = PlayerAttack.Instance.currentItem;
                        Destroy(item.gameObject); 
                        PlayerAttack.Instance.currentItem = null;
                        PlayerAttack.Instance.EquipItem(stackedItem);
                    }
                    else if (canAdd == 3)
                    {
                        item.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
// TO DO
// item position - prefab "hand position"