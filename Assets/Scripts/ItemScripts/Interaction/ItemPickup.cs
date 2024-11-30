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
                    SetItemPosition();
                    if (canAdd == 1) 
                    {
                        InventoryManager.Instance.EquipItem(item);
                    }
                    else if (canAdd == 2) 
                    {
                        Item stackedItem = InventoryManager.Instance.currentItem;
                        Destroy(item.gameObject);
                        InventoryManager.Instance.currentItem = null;
                        InventoryManager.Instance.EquipItem(stackedItem);
                    }
                    else if (canAdd == 3)
                    {
                        item.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void SetItemPosition()
    {
        Transform itemHolder = GameObject.Find("ItemHolder").transform;
        item.transform.SetParent(itemHolder);
        item.transform.localPosition = new Vector3(0.58f, -0.14f, 0.682f);
    }
}
// TO DO
// item position - prefab "hand position"