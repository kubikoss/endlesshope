using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Item item;

    public GameObject dropButton;

    public void SetItem(Item newItem)
    {
        item = newItem;
    }

    public Item GetItem()
    {
        return item;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        dropButton.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dropButton.SetActive(false);
    }
}
