using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }

    public float defaultDamage = 15f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AttackEnemy(GameObject enemy, Item currentItem)
    {
        Enemy enemyHealth = enemy.GetComponent<Enemy>();

        if (enemyHealth != null && !InventoryManager.Instance.isInventoryOpened)
        {
            if (currentItem is Weapon weapon)
            {
                enemyHealth.TakeDamage(weapon.Damage);
            }
            else if(currentItem == null)
            {
                enemyHealth.TakeDamage(defaultDamage);
            }
        }
    }

    public void DefaultAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(PlayerManager.Instance.mainCamera.transform.position, PlayerManager.Instance.mainCamera.transform.forward, out hit, 2f))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    AttackEnemy(hit.collider.gameObject, null);
                }
            }
            Debug.Log("attacking with hands");
        }
    }
}