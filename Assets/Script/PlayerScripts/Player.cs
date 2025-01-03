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
    float hungerTime;
    private float currentHungerTime;
    [SerializeField]
    float hungerDamageInterval;
    private float currentDamageInterval;
    [SerializeField]
    float fatigueTime = 100;
    private float currentFatigue;

    private bool canReduce = true;

    public Slider healthBar;
    public Slider hungerBar;
    public Slider fatigueBar;

    private float originalBaseSpread;
    private float originalMovingSpread;
    private float originalMaxSpread;
    private float originalSpeed;
    private float originalDefaultDamage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        currentHungerTime = hungerTime;
        hungerBar.value = hungerTime;
        currentDamageInterval = hungerDamageInterval;
        healthBar.value = hp;
        currentFatigue = fatigueTime;
        fatigueBar.value = fatigueTime;

        originalSpeed = PlayerMovement.Instance.moveSpeed;
        originalDefaultDamage = PlayerAttack.Instance.defaultDamage;
    }

    void Update()
    {
        IsDead();
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

    private void IsDead()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (hp <= 0)
        {
            Weapon.ammoPools.Clear();
            Weapon.collectedWeapons.Clear();
            SceneManager.LoadScene(activeScene.name);
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        healthBar.value -= amount;
        if (hp <= 0f)
        {
            IsDead();
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
                hungerBar.value = hungerTime;
            }
            else
            {
                TakeDamage(foodItem.foodStat);
            }
        }
    }

    private void UpdateHungerTimer()
    {
        int damage = 10;
        currentDamageInterval -= Time.deltaTime;

        if (currentHungerTime <= hungerTime / 2)
            currentHungerTime -= Time.deltaTime * 2;
        else
            currentHungerTime -= Time.deltaTime;

        if (currentHungerTime <= 0 && currentDamageInterval <= 0)
        {
            TakeDamage(damage);
            currentDamageInterval = hungerDamageInterval;
        }

        hungerBar.value = currentHungerTime;
    }

    private void UpdateFatigueTimer()
    {
        currentFatigue -= Time.deltaTime;
        fatigueBar.value = currentFatigue;

        if (currentFatigue <= 0 && canReduce)
        {
            ApplyFatigue();
            canReduce = false;
        }
        else if (currentFatigue > 0)
        {
            RevertFatigue();
            canReduce = true;
        }
    }

    public void Sleep()
    {
        currentFatigue = fatigueTime;
        fatigueBar.value = currentFatigue;

        //bed game object
    }

    private void ApplyFatigue()
    {
        if (InventoryManager.Instance.currentItem is AK47 ak)
        {
            originalBaseSpread = ak.baseSpread;
            originalMovingSpread = ak.movingSpread;
            originalMaxSpread = ak.maxSpread;

            ak.baseSpread *= 2;
            ak.movingSpread *= 2;
            ak.maxSpread *= 2;
        }

        PlayerMovement.Instance.moveSpeed /= 2;
        PlayerAttack.Instance.defaultDamage /= 2;
    }

    private void RevertFatigue()
    {
        if (InventoryManager.Instance.currentItem is AK47 ak)
        {
            ak.baseSpread = originalBaseSpread;
            ak.movingSpread = originalMovingSpread;
            ak.maxSpread = originalMaxSpread;
        }

        PlayerMovement.Instance.moveSpeed = originalSpeed;
        PlayerAttack.Instance.defaultDamage = originalDefaultDamage;
    }
}