using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isOutputItem || item.ID == 1)
        {
            return;
        }
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        InventoryManager.Instance.isDragging = true;
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
        InventoryManager.Instance.isDragging = false;

        if (!IsMouseInInventory())
        {
            if(count > 1)
            {
                for (int i = 0; i < count; i++)
                {
                    Item itemToWorld = InventoryManager.Instance.InstantiateItem(true, false, item);
                }
            }
            else if(count == 1)
            {
                Item itemToWorld = InventoryManager.Instance.InstantiateItem(true, false, item);
            }
            Destroy(item.gameObject);
            RemoveItemFromInventory();
            InventoryManager.Instance.EquipFirstSlot();
        }
        else
        {
            transform.SetParent(parentAfterDrag);
            InventoryManager.Instance.UpdateHotbar();
            CraftingManager.Instance.CheckCraftingSlotsForRecipes();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        /*if (Input.GetKey(KeyCode.LeftShift))
        {
            if (item != null && count > 1)
            {
                GameObject splitItem = Instantiate(item.ItemWorld, PlayerManager.Instance.player.transform.position, Quaternion.identity);
                splitItem.GetComponent<ItemPickup>().SetItemPosition();
                InventoryManager.Instance.AddItem(splitItem.GetComponent<Item>());
                count--;
                UpdateCount();
            }
        }*/
    }

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

    private bool IsMouseInInventory()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

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
//TODO
// edit splitting item