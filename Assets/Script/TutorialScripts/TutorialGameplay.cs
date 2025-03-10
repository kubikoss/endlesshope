using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGameplay : MonoBehaviour
{
    public static TutorialGameplay Instance { get; private set; }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [HideInInspector] public bool canSpawn = false;
    public bool tutorialCompleted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && tutorialCompleted)
        {
            SceneManager.LoadScene(2);
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