using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item;
    //public Transform itemPosition;

    public void Interact()
    {
        if (item != null)
        {
            InventoryManager.Instance.AddItem(item);
            /*Weapon weaponToEquip = item as Weapon;

            if (weaponToEquip != null)
            {

                if (PlayerAttack.Instance.currentWeapon != null)
                {
                    PlayerAttack.Instance.currentWeapon.gameObject.SetActive(false);
                }

                weaponToEquip.transform.SetParent(itemPosition);

                weaponToEquip.gameObject.SetActive(true);
                PlayerAttack.Instance.EquipWeapon(weaponToEquip);

            }*/
            //Destroy(gameObject);
        }
    }
}
