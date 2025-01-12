using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float lookRadius = 10f;
    [SerializeField]
    private float attackRadius = 2f;
    private bool hasSeen;
    Transform target;
    NavMeshAgent agent;
    EnemyAttack enemyAttack;

    private void Start()
    {
        target = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
        agent.stoppingDistance = attackRadius;
        hasSeen = false;
    }

    private void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius && !hasSeen)
        {
            RaycastHit hit;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, directionToTarget, out hit, lookRadius))
            {
                if (hit.transform == target)
                {
                    hasSeen = true;
                }
            }
        }

        if (hasSeen)
        {
            agent.SetDestination(target.position);
            if (distance <= attackRadius)
            {
                FaceTarget();
                enemyAttack.AttackPlayer(target.gameObject);
            }
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}