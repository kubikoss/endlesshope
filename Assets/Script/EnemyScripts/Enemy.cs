using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float hp = 100;
    public GameObject moneyObject;
    [SerializeField]
    public ParticleSystem particles;

    private NavMeshAgent agent;
    private List<GameObject> itemPool;

    private void Start()
    {
        itemPool = new List<GameObject>(Resources.LoadAll<GameObject>("Items"));
    }

    private void Update()
    {
        IsDead();
    }

    private void IsDead()
    {
        if (hp <= 0)
        {
            AddMoney();

            if (Random.value <= 0.33f)
            {
                SpawnRandomItem();
            }

            Destroy(moneyObject.gameObject);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;

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

    private void SpawnRandomItem()
    {
        if (itemPool.Count == 0)
            return;

        int randomIndex = Random.Range(0, itemPool.Count);
        GameObject randomItem = itemPool[randomIndex];

        GameObject spawnedItem = Instantiate(randomItem, transform.position, Quaternion.identity);
        Item itemName = spawnedItem.GetComponent<Item>();
        itemName.name = Regex.Replace(itemName.ItemName, @"\s*\(.*?\)", "").Trim();
    }
}