using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float hp = 100;

    private void Update()
    {
        isDead();
    }
    public void isDead()
    {
        if (hp <= 0)
        {
            Destroy(gameObject, 0.1f);
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
