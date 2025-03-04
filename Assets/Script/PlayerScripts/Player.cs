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
    public float currentFatigue;
    private AudioSource healthAudioSource;
    [SerializeField]
    AudioClip heartBeat;
    [SerializeField]
    private GameObject deathPanel;

    private bool canReduce = true;
    private bool reduced = false;

    public Slider healthBar;
    public Slider hungerBar;
    public Slider fatigueBar;

    private float originalBaseSpread;
    private float originalMovingSpread;
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

        healthAudioSource = gameObject.AddComponent<AudioSource>();
        healthAudioSource.loop = true;

        deathPanel.gameObject.SetActive(false);

        InventoryManager.Instance.enabled = true;
    }

    void Update()
    {
        IsDead();
        CheckHealth();
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
            //deathPanel.gameObject.SetActive(true);
            Weapon.ammoPools.Clear();
            Weapon.collectedWeapons.Clear();
            SceneManager.LoadScene(activeScene.name);
        }
    }

    public void RestartScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        /*Weapon.ammoPools.Clear();
        Weapon.collectedWeapons.Clear();*/

       /*Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);*/
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

    private void CheckHealth()
    {
        if (hp <= 20 && !healthAudioSource.isPlaying)
        {
            healthAudioSource.clip = heartBeat;
            healthAudioSource.volume = 0.5f;
            healthAudioSource.Play();
        }
        else if (hp > 20 && healthAudioSource.isPlaying)
        {
            healthAudioSource.Stop();
        }
    }

    public void UpdateHunger(Food food)
    {
        if (food.foodItemData != null)
        {
            FoodItem foodItem = food.foodItemData;

            if (foodItem.isEatable)
            {
                if(foodItem.itemName == "Painkillers")
                {
                    currentFatigue += foodItem.foodStat;
                }
                else
                {
                    Heal(foodItem.foodStat);
                    currentHungerTime += foodItem.foodStat / 4 * 3;
                    hungerBar.value = currentHungerTime;
                }
            }
            else
            {
                TakeDamage(foodItem.foodStat);
                currentHungerTime += foodItem.foodStat/2;
                hungerBar.value = currentHungerTime;
            }
            if (currentHungerTime > hungerTime)
                currentHungerTime = hungerTime;
        }
    }

    private void UpdateHungerTimer()
    {
        if (SleepManager.Instance.isSleeping)
            return;

        if (currentHungerTime < 0)
            currentHungerTime = 0;

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
        if (SleepManager.Instance.isSleeping)
            return;

        currentFatigue -= Time.deltaTime;
        fatigueBar.value = currentFatigue;

        if (currentFatigue <= 0 && canReduce)
        {
            ApplyFatigue();
            canReduce = false;
        }
        else if (currentFatigue > 0 && reduced)
        {
            RevertFatigue();
            canReduce = true;
        }
        if(currentFatigue > fatigueTime)
            currentFatigue = fatigueTime;
    }

    public void Sleep(int sleepAmount)
    {
        currentFatigue += sleepAmount;
        if (currentFatigue > fatigueTime)
            currentFatigue = fatigueTime;
        fatigueBar.value = currentFatigue;
        RevertFatigue();
    }

    private void ApplyFatigue()
    {
        AK47[] akWeapons = FindObjectsByType<AK47>(FindObjectsSortMode.None);

        foreach (var ak in akWeapons)
        {
            ak.baseSpread *= 2;
            ak.movingSpread *= 2;
        }

        PlayerMovement.Instance.moveSpeed = 9f;
        PlayerAttack.Instance.defaultDamage = 10f;
        reduced = true;
    }

    private void RevertFatigue()
    {
        AK47[] akWeapons = FindObjectsByType<AK47>(FindObjectsSortMode.None);

        foreach (var ak in akWeapons)
        {
            ak.baseSpread = originalBaseSpread;
            ak.movingSpread = originalMovingSpread;
        }

        PlayerMovement.Instance.moveSpeed = originalSpeed;
        PlayerAttack.Instance.defaultDamage = originalDefaultDamage;
        reduced = false;
    }
}