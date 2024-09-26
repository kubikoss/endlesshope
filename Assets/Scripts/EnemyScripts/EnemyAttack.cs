using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    public float damage = 20f;
    [SerializeField]
    public float attackRate = 1f;
    private float attackCooldown = 0f;

    private void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void AttackPlayer(GameObject player)
    {
        if (attackCooldown <= 0f)
        {
            Player playerHealth = player.GetComponent<Player>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                attackCooldown = 1f / attackRate;
            }
        }
    }
}
