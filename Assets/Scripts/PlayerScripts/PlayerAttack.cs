using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }
    public Item currentItem;
    public Hands hands;

    private float fireTimer = 0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (currentItem == null)
            currentItem = hands;
    }

    private void Update()
    {
        if (!InventoryManager.Instance.isInventoryOpened)
        {
            CurrentItem();
        }
        //SwitchItem();
        //DropItem();
    }

    public void AttackEnemy(GameObject enemy, Item currentItem)
    {
        Enemy enemyHealth = enemy.GetComponent<Enemy>();

        if (enemyHealth != null)
        {
            if (currentItem is Weapon weapon)
            {
                enemyHealth.TakeDamage(weapon.Damage);
            }
        }
    }

    public void EquipItem(Item itemToEquip)
    {
        if (currentItem != null)
        {
            if (!(currentItem is Grenade grenade) || !grenade.IsBeingThrown)
            {
                currentItem.gameObject.SetActive(false);
            }
        }

        currentItem = itemToEquip;

        if (currentItem != null)
        {
            currentItem.gameObject.SetActive(true);
        }
    }

    private void CurrentItem()
    {
        if (currentItem != null)
        {
            if (currentItem is Weapon weapon)
            {
                fireTimer += Time.deltaTime;

                if (weapon.FiringMode == FiringMode.Automatic)
                {
                    if (Input.GetMouseButton(0) && fireTimer >= (1f / weapon.FireRate))
                    {
                        weapon.Shoot();
                        fireTimer = 0f;
                    }
                }
                else if (weapon.FiringMode == FiringMode.SemiAutomatic || weapon.FiringMode == FiringMode.Meelee)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        weapon.Shoot();
                    }
                }

                if (Input.GetKeyDown(KeyCode.R) && weapon != null && !(weapon is Hands) && !(weapon is Grenade))
                {
                    weapon.Reload();
                }
            }
            else if (currentItem is Healable healingItem)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    healingItem.Use();
                }
            }
            else if(currentItem is Food foodItem)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    foodItem.Eat();
                }
            }
        }
    }

    /*private void SwitchItem()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                InventoryManager.Instance.EquipItemFromInventory(i);
                InventoryManager.Instance.ChangeSelectedSlot(i);
            }
        }
    }*/

    /*public void DropItem()
    {
        if (Input.GetKeyDown(KeyCode.G) && !(currentItem is Hands) && currentItem != null)
        {
            Item droppedItem = InventoryManager.Instance.DropItem(currentItem);
            if (droppedItem != null)
            {
                currentItem.gameObject.SetActive(true); 
                currentItem = null;
                InventoryManager.Instance.EquipFirstSlot(); 
            }
        }
    }*/
}