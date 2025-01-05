using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item;

    public void Interact()
    {
        PickupItem();
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
                CheckForAmmo();
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
        //item.transform.localRotation = Quaternion.identity;
    }

    private void CheckForAmmo()
    {
        if(item is Weapon weapon && weapon.weaponData != null && !(weapon is Grenade))
        {
            int fullAmmo = weapon.weaponData.fullAmmo;
            string weaponName = weapon.weaponData.name;

            if (Weapon.collectedWeapons.Contains(item))
                return;
            
            //Weapon.collectedWeapons.Add(item);

            if (Weapon.ammoPools.ContainsKey(weaponName))
                Weapon.ammoPools[weaponName] += fullAmmo;
            else
                Weapon.ammoPools[weaponName] = fullAmmo;
        }
    }
}