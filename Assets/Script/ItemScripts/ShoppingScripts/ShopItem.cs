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

    private void Start()
    {
        costText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.F) && canBuy)
        {
            TryBuyItem();
        }
    }

    private void TryBuyItem()
    {
        if (PlayerCurrency.Instance.CanAfford(itemCost))
        {
            PlayerCurrency.Instance.SpendCurrency(itemCost);
            GameObject shopItem = Instantiate(item.ItemWorld, PlayerManager.Instance.transform.position, Quaternion.identity);
            shopItem.GetComponent<ItemPickup>().Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EnableText();
            canBuy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisableText();
            canBuy = false;
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