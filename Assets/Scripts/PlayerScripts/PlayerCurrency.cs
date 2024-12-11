using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public static PlayerCurrency Instance { get; private set; }

    [SerializeField]
    public InventoryItem currencyInventoryItem;
    [SerializeField]
    public MoneyItem moneyItem;

    [SerializeField]
    private int startCurrency = 5;
    public int currentCurrency = 0;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void Start()
    {
        InitializeCurrency();
    }

    public void InitializeCurrency()
    {
        startCurrency = Mathf.Min(startCurrency, moneyItem.maxAmount);
        currentCurrency = startCurrency;
        //UpdateInventoryCurrency();
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
        //UpdateInventoryCurrency();
    }

    public void SpendCurrency(int amount)
    {
        if(CanAfford(amount))
        {
            currentCurrency -= amount;
            //UpdateInventoryCurrency();
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
        currencyInventoryItem.count = currentCurrency;
    }
}
//TODO
// money inventory item
// starting money
// shopping system