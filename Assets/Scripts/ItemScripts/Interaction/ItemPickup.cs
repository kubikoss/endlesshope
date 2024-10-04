using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item;

    public void Interact()
    {
        if (item != null)
        {
            InventoryManager.Instance.AddItem(item);
            if(InventoryManager.Instance.fullInventory.Count < 9)
            {
                PlayerAttack.Instance.EquipItem(item);
            }
            else
            {
                item.gameObject.SetActive(false);
            }

            Transform weaponHolder = GameObject.Find("InventoryPlaceholder").transform;
            item.transform.SetParent(weaponHolder);
            item.transform.localPosition = new Vector3(0.58f, -0.14f, 0.682f);
        }
    }
}