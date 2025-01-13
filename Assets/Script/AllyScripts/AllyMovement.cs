using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyMovement : MonoBehaviour
{
    public static AllyMovement Instance { get; private set; }
    [SerializeField]
    public float moveSpeed = 9f;
    [SerializeField]
    private float lookRadius = 20f;
    [SerializeField]
    private float attackRadius = 3f;
    [SerializeField]
    private LayerMask enemyLayer;

    private PlayerManager Player;
    private Transform target;
    private NavMeshAgent agent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        Player = FindAnyObjectByType<PlayerManager>();
        target = Player.player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRadius;
        agent.speed = moveSpeed;
    }

    private void Update()
    {
        FindTarget();
    }

    public void FindTarget()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.player.transform.position);

        Collider[] enemies = Physics.OverlapSphere(transform.position, lookRadius, enemyLayer);

        if (enemies.Length > 0)
        {
            Collider closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider enemyCollider in enemies)
            {
                Enemy enemyHealth = enemyCollider.GetComponent<Enemy>();
                if (enemyHealth != null)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemyCollider.transform.position);
                    if (distanceToEnemy < closestDistance)
                    {
                        closestEnemy = enemyCollider;
                        closestDistance = distanceToEnemy;
                    }
                }
            }

            if (closestEnemy != null)
            {
                agent.SetDestination(closestEnemy.transform.position);
                closestEnemy.GetComponent<Enemy>().TakeDamage(1000);
            }
        }
        else
        {
            /*if (distanceToPlayer > lookRadius)
            {
                agent.SetDestination(Player.player.transform.position);
            }
            else
            {
                Patrol();
            }*/
        }
    }

    public void Patrol()
    {
        Debug.Log("Patrolling");
    }
}