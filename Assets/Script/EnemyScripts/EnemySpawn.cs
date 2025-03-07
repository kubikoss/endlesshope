using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    private float currentTimeBetweenSpawn;
    [SerializeField]
    private float timeBetweenSpawn = 20f;
    private float distance;

    private void Start()
    {
        currentTimeBetweenSpawn = timeBetweenSpawn;
    }

    private void Update()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        currentTimeBetweenSpawn -= Time.deltaTime;
        distance = Vector3.Distance(this.transform.position, PlayerManager.Instance.player.transform.position);
        Debug.Log(distance + " " + currentTimeBetweenSpawn);
        if (currentTimeBetweenSpawn <= 0 && distance <= 15f)
        {
            GameObject spawnedEnemy = Instantiate(enemy, this.transform.position, Quaternion.identity);
            currentTimeBetweenSpawn = timeBetweenSpawn;
        }
    }
}