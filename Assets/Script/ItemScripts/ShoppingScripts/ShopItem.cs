using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private int itemCost;
    [SerializeField]
    private Item item;
    [SerializeField]
    private TextMeshProUGUI costText;
    private bool canBuy;
    public bool isLooking;

    private void Start()
    {
        costText.gameObject.SetActive(false);
        canBuy = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canBuy && isLooking)
        {
            TryBuyItem();
        }
    }

    public void TryBuyItem()
    {
        if (PlayerCurrency.Instance.CanAfford(itemCost) && !MenuManager.Instance.isPaused && !SleepManager.Instance.isSleeping)
        {
            PlayerCurrency.Instance.SpendCurrency(itemCost);
            GameObject shopItem = Instantiate(item.ItemWorld, PlayerManager.Instance.transform.position, Quaternion.identity);
            shopItem.AddComponent<ItemPickup>().item = item;
            shopItem.GetComponent<ItemPickup>().Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EnableText();
            canBuy = true;
            costText.text = $"Press F to buy {item.ItemName} for: " + itemCost.ToString() + " money.";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisableText();
            canBuy = false;
            costText.text = "";
        }
    }

    private void EnableText()
    {
        costText.gameObject.SetActive(true);
    }

    private void DisableText()
    {
        costText.gameObject.SetActive(false);
    }
}