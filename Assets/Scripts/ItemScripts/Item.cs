using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public BaseItem itemData;
    public string ItemName => itemData.name;
    public int ID => itemData.ID;
    public Sprite ItemIcon => itemData.itemIcon;
    private Camera playerCamera;
    public Camera PlayerCamera
    {
        get { return playerCamera; }
        set { playerCamera = value; }
    }
    public bool IsStackable => itemData.isStackable;

    public GameObject HandItem => itemData.handItem;
    private void Awake()
    {
        playerCamera = Camera.main;
    }

    public void UpdateInventoryItem()
    {
        InventoryItem inventoryItem = InventoryManager.Instance.GetInventoryItem(this);
        if (inventoryItem != null)
        {
            inventoryItem.count--;
            inventoryItem.UpdateCount();
            Debug.Log(inventoryItem.count);

            if (inventoryItem.count < 1)
            {
                InventoryManager.Instance.RemoveItem(this);
                InventoryManager.Instance.EquipFirstSlot();
                Destroy(inventoryItem.gameObject);
            }
        }
    }
}