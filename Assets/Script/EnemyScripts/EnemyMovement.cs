using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float lookRadius = 10f;
    [SerializeField]
    private float attackRadius = 2f;
    private bool hasSeen = false;

    Transform playerTarget;
    Transform allyTarget;
    NavMeshAgent agent;
    EnemyAttack enemyAttack;
    Animator animator;

    private Vector3 lastPosition;

    private void Start()
    {
        playerTarget = PlayerManager.Instance.player.transform;
        allyTarget = AllyController.Instance.ally.transform;

        enemyAttack = GetComponent<EnemyAttack>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = attackRadius;

        hasSeen = false;
        lastPosition = transform.position;
    }


    private void Update()
    {
        CheckTarget();
        EnemyMovementAnimation();
    }

    private Transform GetClosestTarget()
    {
        if (allyTarget == null)
            return playerTarget;

        Transform closestTarget = null;
        float distanceToPlayer = playerTarget != null ? Vector3.Distance(playerTarget.position, transform.position) : 0;
        float distanceToAlly = allyTarget != null ? Vector3.Distance(allyTarget.position, transform.position) : 0;

        if (playerTarget != null)
            distanceToPlayer = Vector3.Distance(playerTarget.position, transform.position);

        if (allyTarget != null)
            distanceToAlly = Vector3.Distance(allyTarget.position, transform.position);

        if (distanceToPlayer < distanceToAlly)
            closestTarget = playerTarget.transform;
        else
            closestTarget = allyTarget.transform;

        return closestTarget;
    }

    private void CheckTarget()
    {
        Transform target = GetClosestTarget();
        if (target == null) return;

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
        AttackTarget(distance, target);
    }

    private void AttackTarget(float distance, Transform target)
    {
        if (hasSeen)
        {
            agent.SetDestination(GetClosestTarget().position);
            if (distance <= attackRadius)
            {
                FaceTarget(target);

                if (target.transform.CompareTag("Player"))
                    enemyAttack.AttackPlayer(playerTarget.gameObject);
                else if (target.transform.CompareTag("Ally"))
                    enemyAttack.AttackAlly(allyTarget.gameObject);
            }
        }
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void EnemyMovementAnimation()
    {
        bool isMoving = Vector3.Distance(transform.position, lastPosition) > 0f;
        lastPosition = transform.position;

        animator.SetBool("isMoving", isMoving);

        if (!isMoving)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}