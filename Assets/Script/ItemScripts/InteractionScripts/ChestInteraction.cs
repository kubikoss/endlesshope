using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ChestInteraction : MonoBehaviour, IInteractable
{
    private System.Random rndNumber = new System.Random();
    public int maxItemsCount = 3;
    private bool isOpened = false;
    [SerializeField]
    private float range = 3f;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject itemCanvas;

    private List<GameObject> items;

    private void Start()
    {
        items = new List<GameObject>(Resources.LoadAll<GameObject>("Items"));

        int itemCount = rndNumber.Next(1, maxItemsCount + 1);
        AddToChest(itemCount);
        text.gameObject.SetActive(false);
    }

    private void Update()
    {
        Interact();
        IsClose();
    }

    public void Interact()
    {
        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);

        if (!isOpened && Input.GetKeyDown(KeyCode.E) && distance <= range)
        {
            isOpened = true;

            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform item = transform.GetChild(0);
                item.name = Regex.Replace(item.name, @"\s*\(.*?\)", "").Trim();
                item.position = transform.position;
                item.SetParent(null);
                item.gameObject.SetActive(true);
            }
            Destroy(itemCanvas);
            Destroy(gameObject);
        }
    }

    private void AddToChest(int amount)
    {
        List<GameObject> shuffledItems = new List<GameObject>(items);
        for (int i = 0; i < shuffledItems.Count; i++)
        {
            int randomIndex = rndNumber.Next(i, shuffledItems.Count);
            GameObject temp = shuffledItems[i];
            shuffledItems[i] = shuffledItems[randomIndex];
            shuffledItems[randomIndex] = temp;
        }

        for (int i = 0; i < Mathf.Min(amount, shuffledItems.Count); i++)
        {
            GameObject randomItem = shuffledItems[i];
            GameObject itemInstance = Instantiate(randomItem, transform.position, Quaternion.identity);
            itemInstance.transform.SetParent(transform);
            itemInstance.gameObject.SetActive(false);
        }
    }

    private void IsClose()
    {
        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);
        if (distance <= range)
        {
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}