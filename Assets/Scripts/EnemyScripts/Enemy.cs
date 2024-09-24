using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float hp = 100;

    void Start()
    {

    }

    void Update()
    {

    }

    public void isDead()
    {
        Destroy(gameObject, 0.2f);
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
