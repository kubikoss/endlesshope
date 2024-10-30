using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item;
    [HideInInspector] public InventoryItem inventoryItem;
    public void Interact()
    {
        if(!InventoryManager.Instance.isInventoryOpened)
        {
            if (item != null)
            {
                bool canAdd = InventoryManager.Instance.AddItem(item);
                if (canAdd)
                {
                    Transform itemHolder = GameObject.Find("ItemHolder").transform;
                    item.transform.SetParent(itemHolder);
                    item.transform.localPosition = new Vector3(0.58f, -0.14f, 0.682f);

                    if (InventoryManager.Instance.GetHotbarCount())
                    {
                        
                        PlayerAttack.Instance.EquipItem(item);
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}