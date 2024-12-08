using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float hp = 100;
    bool itsDead = false;
    public MoneyItem moneyData;

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
            itsDead = true;
            isDead();
        }
    }
}
//TODO
// spawn/add money after hp = 0;