using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float hp = 100;

    void Update()
    {
        isDead();
        Debug.Log(hp);
    }

    public void Heal(float healAmount)
    {
        hp += healAmount;

        if (hp > 100)
        {
            hp = 100;
        }
    }

    public void isDead()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (hp <= 0)
        {
            SceneManager.LoadScene(activeScene.name);
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0f)
        {
            isDead();
        }
    }
}
