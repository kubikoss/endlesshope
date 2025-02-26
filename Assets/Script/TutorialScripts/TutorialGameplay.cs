using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameplay : MonoBehaviour
{
    public static TutorialGameplay Instance { get; private set; }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [HideInInspector] public bool canSpawn = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SpawnEnemy()
    {
        if (canSpawn && enemyPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            }
            canSpawn = false;
        }
    }
}