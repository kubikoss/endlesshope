using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public TextMeshProUGUI countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public bool isOutputItem = false;

    public void DisplayItemInInventory(Item newItem, bool isOutput = false)
    {
        item = newItem;
        image.sprite = newItem.ItemIcon;
        isOutputItem = isOutput;
        UpdateCount();
    }

    public void RemoveItemFromInventory()
    {
        item = null;
        image.sprite = null;
        Destroy(gameObject);
    }

    public void UpdateCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isOutputItem || item.ID == 1)
        {
            return;
        }
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isOutputItem || item.ID == 1)
        {
            return;
        }
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isOutputItem || item.ID == 1)
        {
            return;
        }
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);

        InventoryManager.Instance.UpdateHotbar();
        CraftingManager.Instance.CheckCraftingSlotsForRecipes();
    }
}