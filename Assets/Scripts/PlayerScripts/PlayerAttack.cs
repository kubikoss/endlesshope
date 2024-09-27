using System.Collections;
using System.Collections.Generic;
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
    }
    private void Start()
    {
        if (currentItem == null)
        {
            currentItem = hands;
        }
    }

    private void Update()
    {
        CurrentItem();
        SwitchItem();
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
            currentItem.gameObject.SetActive(false);
        }

        currentItem = itemToEquip;

        Transform weaponHolder = GameObject.Find("WeaponHolder").transform;
        currentItem.transform.SetParent(weaponHolder);
        currentItem.transform.localPosition = new Vector3(0.58f, -0.14f, 0.682f);
        currentItem.gameObject.SetActive(true);
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

                if (Input.GetKeyDown(KeyCode.R) && weapon != null && !(weapon is Hands))
                {
                    weapon.Reload();
                }
            }
            else if(currentItem is Healable healingItem)
            {

            }
        }
    }

    private void SwitchItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItem(hands);
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2 + i))
            {
                InventoryManager.Instance.EquipItemFromInventory(i);
            }
        }
    }
}
