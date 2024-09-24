using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }
    public Weapon currentWeapon;

    private float fireTimer = 0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Update()
    {
        if (currentWeapon != null)
        {
            fireTimer += Time.deltaTime;

            if (currentWeapon.FiringMode == FiringMode.Automatic)
            {
                if (Input.GetMouseButton(0) && fireTimer >= (1f / currentWeapon.FireRate))
                {
                    currentWeapon.Shoot();
                    fireTimer = 0f;
                }
            }
            else if (currentWeapon.FiringMode == FiringMode.SemiAutomatic || currentWeapon.FiringMode == FiringMode.Meelee)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentWeapon.Shoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentWeapon != null && !(currentWeapon is Hands))
        {
            currentWeapon.Reload();
        }
    }

    public void AttackEnemy(GameObject enemy, Weapon weapon)
    {
        Enemy enemyHealth = enemy.GetComponent<Enemy>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(weapon.Damage);
        }
    }

    public void EquipWeapon(Weapon weaponToEquip)
    {
        currentWeapon = weaponToEquip;
    }
}
