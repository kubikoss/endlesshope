using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{
    public static PlayerCurrency Instance { get; private set; }

    [SerializeField]
    private Sprite image;
    [SerializeField]
    private TextMeshProUGUI moneyCount;
    [SerializeField]
    public MoneyItem moneyItem;

    [SerializeField]
    private int startCurrency = 5;
    [HideInInspector] public int currentCurrency = 0;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        InitializeCurrency();
    }

    private void InitializeCurrency()
    {
        startCurrency = Mathf.Min(startCurrency, moneyItem.maxAmount);
        currentCurrency = startCurrency;
        UpdateInventoryCurrency();
    }
    
    public void AddCurrency(int amount)
    {
        if(currentCurrency + amount <= moneyItem.maxAmount)
        {
            currentCurrency += amount;
        }
        else
        {
            currentCurrency = moneyItem.maxAmount;
        }
        UpdateInventoryCurrency();
    }

    public void SpendCurrency(int amount)
    {
        if(CanAfford(amount))
        {
            currentCurrency -= amount;
            UpdateInventoryCurrency();
        }
    }

    public bool CanAfford(int itemCost)
    {
        if (currentCurrency >= itemCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateInventoryCurrency()
    {
        moneyCount.text = currentCurrency.ToString();
    }
}