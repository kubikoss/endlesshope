using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
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
                AudioManager.Instance.PickupItemSound();

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
        item.transform.SetParent(itemHolder, true);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = item.GetComponent<Item>().SetHandRotation();

        if (item.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            Destroy(rb);
        }
        if(item.GetComponent<Collider>() != null)
        {
            Collider cd = item.GetComponent<Collider>();
            cd.isTrigger = true;
        }
    }

    private void CheckForAmmo()
    {
        if(item is Weapon weapon && weapon.weaponData != null && !(weapon is Grenade))
        {
            int fullAmmo = weapon.weaponData.fullAmmo;
            FiringMode firingMode = weapon.weaponData.firingMode;

            if (Weapon.collectedWeapons.Contains(item))
                return;
            
            Weapon.collectedWeapons.Add(item);

            if (Weapon.ammoPools.ContainsKey(firingMode))
                Weapon.ammoPools[firingMode] += fullAmmo;
            else
                Weapon.ammoPools[firingMode] = fullAmmo;
        }
    }
}