using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item
{
    public MoneyItem moneyData;

    private void Start()
    {
        moneyData = ScriptableObject.CreateInstance<MoneyItem>();
        moneyData.amount = AmountProbability();
        this.itemData = moneyData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            AddCurrency();
        }
    }

    public void AddCurrency()
    {
        PlayerCurrency.Instance.AddCurrency(moneyData.amount);
        Debug.Log(PlayerCurrency.Instance.currentCurrency);
        Destroy(gameObject);
    }

    private int AmountProbability()
    {
        float chance = Random.Range(0f, 1f);

        if(chance <= 0.7f)
        {
            return Random.Range(1, 8);
        }
        else if (chance <= 0.9f)
        {
            return Random.Range(8, 14);
        }
        else
        {
            return Random.Range(14, 21);
        }
    }
}