using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AllyController : MonoBehaviour
{
    public static AllyController Instance { get; private set; }
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 9f;
    [SerializeField]
    private float enemyLookRadius = 20f;
    [SerializeField]
    private float playerLookRadius = 7f;
    [SerializeField]
    private LayerMask enemyLayer;

    [Header("Attack")]
    [SerializeField]
    private float attackRadius = 3f;
    [SerializeField]
    private float attackCooldown = 2f;
    [SerializeField]
    private float damage = 30f;

    [SerializeField]
    private float hp = 100f;

    private NavMeshAgent agent;
    private Transform playerTarget;

    private float currentCooldown;
    private bool isPatrolling;

    public GameObject ally;
    public AudioClip clip;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        playerTarget = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRadius;
    }

    private void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;

        AllyActions();
    }

    private void AllyActions()
    {
        Collider closestEnemy = FindClosestEnemy();
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (closestEnemy != null)
        {
            EnemyTakeDamage(closestEnemy);
        }
        else if (distanceToPlayer > playerLookRadius)
        {
            agent.SetDestination(playerTarget.position);
        }
        else
        {
            Patrol();
        }
    }

    private Collider FindClosestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, enemyLookRadius, enemyLayer);

        Collider closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemyCollider in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestEnemy = enemyCollider;
                closestDistance = distanceToEnemy;
            }
        }
        return closestEnemy;
    }

    public void EnemyTakeDamage(Collider enemyCollider)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, enemyCollider.transform.position);
        agent.SetDestination(enemyCollider.transform.position);

        if (distanceToEnemy <= attackRadius && currentCooldown <= 0)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            FaceTarget(enemy.transform);
            if (enemy != null)
            {
                AudioManager.Instance.PlayAudio(clip, 0.2f);
                enemy.TakeDamage(damage);
                currentCooldown = attackCooldown;
            }
        }
    }

    public void AllyTakeDamage(float amount)
    {
        hp -= amount;

        if (hp <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void Patrol()
    {
        if (isPatrolling)
            return;

        StartCoroutine(PatrolRoutine());
    }

    private IEnumerator PatrolRoutine()
    {
        isPatrolling = true;

        Vector3 randomDirection = Random.insideUnitSphere * playerLookRadius + transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, playerLookRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(2f);
        isPatrolling = false;
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}