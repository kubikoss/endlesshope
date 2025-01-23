using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 100;
    public GameObject moneyObject;
    [SerializeField]
    public ParticleSystem particles;
    
    private void Update()
    {
        IsDead();
    }

    private void IsDead()
    {
        if (hp <= 0)
        {
            AddMoney();
            Destroy(moneyObject.gameObject);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        //ParticleManager.Instance.SpawnParticles(particles, transform, 0.5f);

        if (hp <= 0f)
        {
            IsDead();
        }
    }

    private void AddMoney()
    {
        moneyObject = Instantiate(moneyObject, transform.position, Quaternion.identity);
        Money moneyScript = moneyObject.GetComponent<Money>();
        moneyScript.moneyData.amount = moneyScript.AmountProbability();
        PlayerCurrency.Instance.AddCurrency(moneyScript.moneyData.amount);
    }

}