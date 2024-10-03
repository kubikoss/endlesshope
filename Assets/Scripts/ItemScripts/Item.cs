using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Item : MonoBehaviour //, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BaseItem itemData;
    public string itemName => itemData.name;
    /*public Sprite icon;

    private CanvasGroup canvas;
    private Transform originalParent;*/

    private Camera playerCamera;
    public Camera PlayerCamera
    {
        get { return playerCamera; }
        set { playerCamera = value; }
    }

    private void Awake()
    {
        /*canvas = GetComponent<CanvasGroup>();
        originalParent = transform.parent;*/

        playerCamera = Camera.main;
    }
    #region item dragging

    /*public void OnBeginDrag(PointerEventData eventData)
    {
        canvas.alpha = 0.06f;
        canvas.blocksRaycasts = false;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvas.alpha = 1f;
        canvas.blocksRaycasts = true;

        if(eventData.pointerCurrentRaycast.isValid)
        {
            InventorySlot targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();

            if(targetSlot != null)
            {
                Item targetItem= targetSlot.GetItem();
                if (targetItem != null)
                {
                    InventoryManager.Instance.MoveItem(this, targetItem);
                }
                else
                {
                    transform.SetParent(targetSlot.transform);
                    targetSlot.SetItem(this);
                }
            }
            else
            {
                transform.SetParent(originalParent);
            }
        }
        else
        {
            transform.SetParent(originalParent);
        }
    }*/
    #endregion
}