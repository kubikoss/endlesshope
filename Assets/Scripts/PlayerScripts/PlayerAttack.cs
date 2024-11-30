using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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
}