using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image image;
    public TextMeshProUGUI countText;

    public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public bool isSplitting = false;
    public GameObject imageBackGround;
    public bool isOutputItem = false;

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
        if (isOutputItem || SleepManager.Instance.isSleeping)
            return;

        if (MenuManager.Instance.isPaused)
        {
            transform.position = parentAfterDrag.position;
            transform.SetParent(parentAfterDrag);
            InventoryManager.Instance.isDragging = false;
            image.raycastTarget = true;
            return;
        }

        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        InventoryManager.Instance.isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isOutputItem || SleepManager.Instance.isSleeping)
            return;

        if (MenuManager.Instance.isPaused)
        {
            transform.position = parentAfterDrag.position;
            transform.SetParent(parentAfterDrag);
            InventoryManager.Instance.isDragging = false;
            image.raycastTarget = true;
            return;
        }

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isOutputItem || SleepManager.Instance.isSleeping)
            return;

        if (MenuManager.Instance.isPaused)
        {
            transform.position = parentAfterDrag.position;
            transform.SetParent(parentAfterDrag);
            InventoryManager.Instance.isDragging = false;
            image.raycastTarget = true;
            return;
        }

        image.raycastTarget = true;
        InventoryManager.Instance.isDragging = false;
        CheckItemOutsideInventory();
        CraftingManager.Instance.CheckCraftingSlotsForRecipes();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && count > 1)
        {
            int itemCount = 0;
            foreach(InventorySlot slot in InventoryManager.Instance.inventorySlots)
            {
                if(slot.transform.childCount <= 0)
                {
                    itemCount++;  
                }
            }

            if(itemCount > 0)
            {
                InventoryManager.Instance.splitting = true;
                Item splitItem = Instantiate(item, Vector3.zero, Quaternion.identity);
                splitItem.GetComponent<ItemPickup>().Interact();
                count--;
                UpdateCount();
                InventoryManager.Instance.splitting = false;
                CraftingManager.Instance.CheckCraftingSlotsForRecipes();
            }
        }
    }

    private void CheckItemOutsideInventory()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // Inventory item outside of slot, dropped => equipped hands
        if (!IsMouseInInventory(pointerData))
        {
            if(item is Weapon weapon && (weapon is AK47 || weapon is Glock))
            {
                item.transform.SetParent(null);
                item.transform.position = PlayerManager.Instance.player.transform.position;
                InventoryManager.Instance.currentItem = null;
                InventoryManager.Instance.DropItemRigidBody(item);
            }
            else
            {
                if (count > 1)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Item itemToWorld = InventoryManager.Instance.InstantiateItem(true, false, item);
                    }
                }
                else if (count == 1)
                {
                    Item itemToWorld = InventoryManager.Instance.InstantiateItem(true, false, item);
                }
                Destroy(item.gameObject);
            }
            AudioManager.Instance.DropItemAudio();
            RemoveItemFromInventory();
            InventoryManager.Instance.hotbarCount--;
            
            for(int i = 0; i < InventoryManager.Instance.hotbarCount; i++)
            {
                InventoryItem itemInSlot = InventoryManager.Instance.inventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null)
                {
                    InventoryManager.Instance.EquipItem(itemInSlot.item);
                    InventoryManager.Instance.ChangeSelectedSlot(i);
                    break;
                }
            }
        }
        // Inventory item put in slot
        else
        {
            transform.SetParent(parentAfterDrag);
            InventoryManager.Instance.UpdateHotbar();
        }
    }

    private bool IsMouseInInventory(PointerEventData pointerData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("InventorySlot") || result.gameObject.CompareTag("Inventory") || result.gameObject.CompareTag("Crafting"))
            {
                return true;
            }
        }
        return false;
    }
}