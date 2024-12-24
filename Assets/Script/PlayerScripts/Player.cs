using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField]
    public float hp = 100;
    [SerializeField]
    public float hungerTime;
    private float currentHungerTime;
    [SerializeField]
    private float hunger = 100;
    [SerializeField]
    public float fatigueTime = 100;
    private float currentFatigue;

    public Slider healthBar;
    public Slider hungerBar;
    public Slider fatigueBar;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        currentHungerTime = hungerTime;
        hungerBar.value = hunger;
        healthBar.value = hp;
        currentFatigue = fatigueTime;
        fatigueBar.value = fatigueTime;
    }

    void Update()
    {
        isDead();
        UpdateHungerTimer();
        UpdateFatigueTimer();
    }

    public void Heal(float healAmount)
    {
        hp += healAmount;
        healthBar.value += healAmount;
        if (hp > 100)
        {
            hp = 100;
            healthBar.value = hp;
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
        healthBar.value -= amount;
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
                hunger = currentHungerTime;
                hungerBar.value = hunger;
            }
            else
            {
                TakeDamage(foodItem.foodStat);
            }
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
        hunger = currentHungerTime;
        hungerBar.value = hunger;
    }

    private void UpdateFatigueTimer()
    {
        currentFatigue -= Time.deltaTime;
        fatigueBar.value = currentFatigue;
    }
}
//TODO
// sleep bar