using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

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

        if (transform.childCount == 0)
        {
            draggedItem.parentAfterDrag = transform;
            if (PlayerAttack.Instance.currentItem == draggedItem.item)
            {
                InventoryManager.Instance.EquipFirstSlot();
            }
        }
        else if (transform.childCount == 1)
        {
            InventoryItem itemInSlot = transform.GetChild(0).GetComponent<InventoryItem>();
            if (draggedItem.item.ID != 1 && itemInSlot.item.ID != 1)
            {
                //swap parents
                Transform originalParent = draggedItem.parentAfterDrag;
                draggedItem.parentAfterDrag = itemInSlot.transform.parent;
                itemInSlot.parentAfterDrag = originalParent;

                //change place in inventory
                draggedItem.transform.SetParent(draggedItem.parentAfterDrag);
                itemInSlot.transform.SetParent(itemInSlot.parentAfterDrag);

                // ~
                draggedItem.transform.position = draggedItem.parentAfterDrag.position;
                itemInSlot.transform.position = itemInSlot.parentAfterDrag.position;

                SwapEquippedItem(draggedItem, itemInSlot);
            }
        }
    }

    private void SwapEquippedItem(InventoryItem draggedItem, InventoryItem itemInSlot)
    {
        if (PlayerAttack.Instance.currentItem != null)
        {
            if (PlayerAttack.Instance.currentItem == draggedItem.item)
            {
                PlayerAttack.Instance.currentItem.gameObject.SetActive(false);
                PlayerAttack.Instance.currentItem = itemInSlot.item;
                PlayerAttack.Instance.currentItem.gameObject.SetActive(true);
                PlayerAttack.Instance.EquipItem(itemInSlot.item);
            }
            else if (PlayerAttack.Instance.currentItem == itemInSlot.item)
            {
                PlayerAttack.Instance.currentItem.gameObject.SetActive(false);
                PlayerAttack.Instance.currentItem = draggedItem.item;
                PlayerAttack.Instance.currentItem.gameObject.SetActive(true);
                PlayerAttack.Instance.EquipItem(draggedItem.item);
            }
        }
    }
    //TODO
    // stack system
    // ~ crafting
}