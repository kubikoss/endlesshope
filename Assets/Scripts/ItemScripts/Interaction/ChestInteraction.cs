using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : MonoBehaviour, IInteractable
{
    private System.Random rndNumber = new System.Random();
    public int maxItemsCount = 3;
    private bool isOpened = false;

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;
    public GameObject item6;

    private List<GameObject> items;

    private void Start()
    {
        items = new List<GameObject> { item1, item2, item3, item4, item5, item6 };

        int itemCount = rndNumber.Next(1, maxItemsCount + 1);
        AddToChest(itemCount);
    }

    private void Update()
    {
        Interact();
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        Interact();
    }*/
    public void Interact()
    {
        if (!isOpened && Input.GetKeyDown(KeyCode.E))
        {
            isOpened = true;

            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform item = transform.GetChild(0);
                item.position = transform.position;
                item.SetParent(null);
                item.gameObject.SetActive(true);
            }

            Destroy(gameObject);
        }
    }

    public void AddToChest(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject randomItem = items[rndNumber.Next(0, items.Count)];
            GameObject itemInstance = Instantiate(randomItem, transform.position, Quaternion.identity);
            itemInstance.transform.SetParent(transform);
            itemInstance.gameObject.SetActive(false);
        }
    }
}