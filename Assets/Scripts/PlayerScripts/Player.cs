using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int hp = 100;

    void Start()
    {

    }

    void Update()
    {
        isDead();
    }

    public void isDead()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (hp <= 0)
        {
            SceneManager.LoadScene(activeScene.name);
        }
    }
}
