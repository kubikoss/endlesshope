using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField]
    public float hp = 100;
    [SerializeField]
    public float hungerTime;
    private float currentHungerTime;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        currentHungerTime = hungerTime;
    }

    void Update()
    {
        isDead();
        UpdateHungerTimer();
        //Debug.Log(currentHungerTime);
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

    public void UpdateHunger(Food food)
    {
        if (food.foodItemData != null)
        {
            FoodItem foodItem = food.foodItemData;

            if (foodItem.isEatable)
            {
                Heal(foodItem.foodStat);
                currentHungerTime = hungerTime;
            }
            else
            {
                TakeDamage(foodItem.foodStat);
            }
            InventoryManager.Instance.RemoveItem(food);
            InventoryManager.Instance.EquipFirstSlot();
        }
    }

    private void UpdateHungerTimer()
    {
        currentHungerTime -= Time.deltaTime;

        if (currentHungerTime <= 0)
        {
            TakeDamage(10);
            currentHungerTime = hungerTime;
        }
    }
}