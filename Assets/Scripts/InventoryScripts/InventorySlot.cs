using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    public bool isOutputSlot = false;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (isOutputSlot || (draggedItem != null && draggedItem.isOutputItem))
        {
            return;
        }

        if (transform.childCount == 0)
        {
            draggedItem.parentAfterDrag = transform;
            if (InventoryManager.Instance.currentItem == draggedItem.item)
            {
                InventoryManager.Instance.EquipFirstSlot();
            }
        }
        else if (transform.childCount == 1)
        {
            InventoryItem itemInSlot = transform.GetChild(0).GetComponent<InventoryItem>();

            if (draggedItem.item.ID != 1 && itemInSlot.item.ID != 1)
            {
                // Swap parents
                Transform originalParent = draggedItem.parentAfterDrag;
                draggedItem.parentAfterDrag = itemInSlot.transform.parent;
                itemInSlot.parentAfterDrag = originalParent;

                // Change place in the inventory (parents)
                draggedItem.transform.SetParent(draggedItem.parentAfterDrag);
                itemInSlot.transform.SetParent(itemInSlot.parentAfterDrag);

                // Change place in the inventory (visual)
                draggedItem.transform.position = draggedItem.parentAfterDrag.position;
                itemInSlot.transform.position = itemInSlot.parentAfterDrag.position;

                SwapEquippedItem(draggedItem, itemInSlot);
            }
        }
    }

    private void SwapEquippedItem(InventoryItem draggedItem, InventoryItem itemInSlot)
    {
        if (InventoryManager.Instance.currentItem != null)
        {
            if (InventoryManager.Instance.currentItem == draggedItem.item)
            {
                InventoryManager.Instance.currentItem.gameObject.SetActive(false);
                InventoryManager.Instance.currentItem = itemInSlot.item;
                InventoryManager.Instance.currentItem.gameObject.SetActive(true);
                InventoryManager.Instance.EquipItem(itemInSlot.item);
            }
            else if (InventoryManager.Instance.currentItem == itemInSlot.item)
            {
                InventoryManager.Instance.currentItem.gameObject.SetActive(false);
                InventoryManager.Instance.currentItem = draggedItem.item;
                InventoryManager.Instance.currentItem.gameObject.SetActive(true);
                InventoryManager.Instance.EquipItem(draggedItem.item);
            }
        }
    }
}