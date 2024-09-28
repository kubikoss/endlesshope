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
            PlayerAttack.Instance.EquipItem(item);

            Transform weaponHolder = GameObject.Find("WeaponHolder").transform;
            item.transform.SetParent(weaponHolder);
            item.transform.localPosition = new Vector3(0.58f, -0.14f, 0.682f);
        }
    }
}
