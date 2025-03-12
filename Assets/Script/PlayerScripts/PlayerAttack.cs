using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }

    public float defaultDamage = 15f;
    public float defaultAttackRange = 2f;

    private Animator animator;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        animator = GetComponent<Animator>();
    }

    public void AttackEnemy(GameObject enemy, Item currentItem)
    {
        if (Player.Instance.endPanel.activeSelf || Player.Instance.deathPanel.activeSelf)
            return;

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
            animator.SetTrigger("Attack");
            RaycastHit hit;
            if (Physics.Raycast(PlayerManager.Instance.mainCamera.transform.position, PlayerManager.Instance.mainCamera.transform.forward, out hit, defaultAttackRange))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    AttackEnemy(hit.collider.gameObject, null);
                    ParticleManager.Instance.SpawnParticles(hit.collider.GetComponent<Enemy>().particles, hit.point, 0.3f);
                }
            }
        }
    }
}