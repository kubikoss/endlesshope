using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    public float lookRadius = 10f;
    [SerializeField]
    public float attackRadius = 2f;
    Transform target;
    NavMeshAgent agent;
    EnemyAttack enemyAttack;

    void Start()
    {
        target = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
        agent.stoppingDistance = attackRadius;

    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
                RaycastHit hit;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                if (Physics.Raycast(transform.position, directionToTarget, out hit, distance))
                {
                    if (hit.transform == target)
                    {

                        enemyAttack.AttackPlayer(target.gameObject);
                    }
                    else
                    {
                        Debug.Log("neni to hrac");
                    }
                }

            }
        }
    }

    void FaceTarget()
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
