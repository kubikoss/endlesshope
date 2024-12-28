using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item;

    public void Interact()
    {
        if(CraftingManager.Instance.isCrafted)
        {
            PickupItem();
        }
        else
        {
            /*if (!InventoryManager.Instance.isInventoryOpened || !CraftingManager.Instance.isCraftingPanelOpened)
            {
                PickupItem();
            }*/
            PickupItem();
        }
    }

    private void PickupItem()
    {
        if (item != null)
        {
            int canAdd = InventoryManager.Instance.AddItem(item);

            if (canAdd > 0)
            {
                item.name = Regex.Replace(item.name, @"\s*\(.*?\)", "").Trim();

                SetItemPosition();
                if (canAdd == 1)
                {
                    InventoryManager.Instance.EquipItem(item);
                }
                else if (canAdd == 2)
                {
                    Destroy(item.gameObject);
                }
                else if (canAdd == 3)
                {
                    item.gameObject.SetActive(false);
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