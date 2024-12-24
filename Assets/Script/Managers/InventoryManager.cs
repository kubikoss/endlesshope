using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory")]
    public List<InventorySlot> inventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject fullInventory;
    public bool isInventoryOpened = false;
    public bool isDragging = false;
    int selectedSlot = -1;
    [HideInInspector] public int hotbarCount = 0;

    [Header("Item")]
    public Item currentItem;
    public Hands hands;

    [Header("Player")]
    public Player player;
    public PlayerCam playerCam;

    private float fireTimer = 0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (currentItem == null)
            currentItem = hands;

        AddItem(hands);
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        SwitchItem();
        OpenInventory();
        HandleCurrentItem();

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem(currentItem);
        }
    }

    #region add/remove from inventory
    public int AddItem(Item item)
    {
        // Stacking item
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item.ID == item.ID && itemInSlot.count < item.MaxStackCount)
            {
                if (itemInSlot.item.IsStackable && item.IsStackable)
                {
                    itemInSlot.count++;
                    itemInSlot.UpdateCount();
                    Destroy(item.gameObject);

                    EquipItemFromInventory(i);
                    return 2; // Item stacked
                }
            }
        }

        // Empty slot in hotbar
        for (int i = 0; i < 9; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                if (hotbarCount < 9) // Limit
                {
                    hotbarCount++;
                    SpawnToInventory(item, slot);
                    return 1; // Item equipped
                }
            }
        }

        // No empty slot in hotbar
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                SpawnToInventory(item, slot);
                return 3; // Item put in inventory, but not equipped
            }
        }

        return -1; // Full inventory
    }

    public void DropItem(Item itemToDrop)
    {
        if (!(itemToDrop is Hands) && itemToDrop != null)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

                if (itemInSlot != null && itemInSlot.item == itemToDrop)
                {
                    if(itemInSlot.count == 1)
                    {
                        itemInSlot.RemoveItemFromInventory();
                        itemToDrop.transform.SetParent(null);
                        currentItem = null;
                        EquipFirstSlot();
                        hotbarCount--;
                        DropItemRigidBody(itemToDrop);
                    }
                    else if(itemInSlot.count > 1)
                    {
                        Item itemToWorld = InstantiateItem(true, false, itemToDrop);
                        currentItem = null;
                        EquipItem(itemToDrop);
                        itemInSlot.count--;
                        itemInSlot.UpdateCount();
                    }
                }
            }
        }
    }

    private void DropItemRigidBody(Item item)
    {
        Vector3 cameraForward = playerCam.transform.forward;
        cameraForward.y = 2f;
        cameraForward.Normalize();

        if(item.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = item.AddComponent<Rigidbody>();
            rb.AddForce(cameraForward * 5f, ForceMode.Impulse);
        }
    }

    private void SpawnToInventory(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.DisplayItemInInventory(item);

        int invSlot = inventorySlots.IndexOf(slot);
        ChangeSelectedSlot(invSlot);
    }
    #endregion
    #region item equipping/switching
    public void EquipItem(Item itemToEquip)
    {
        if (currentItem != null)
        {
            if (!(currentItem is Grenade grenade) || !grenade.IsBeingThrown)
            {
                currentItem.gameObject.SetActive(false);
            }
        }

        currentItem = itemToEquip;

        if (currentItem != null)
        {
            currentItem.gameObject.SetActive(true);
        }
    }

    private void HandleCurrentItem()
    {
        if (currentItem != null && !isInventoryOpened)
        {
            if (currentItem is Weapon weapon)
            {
                fireTimer += Time.deltaTime;

                if (weapon.FiringMode == FiringMode.Automatic)
                {
                    if (Input.GetMouseButton(0) && fireTimer >= (1f / weapon.FireRate))
                    {
                        weapon.Shoot();
                        fireTimer = 0f;
                    }
                }
                else if (weapon.FiringMode == FiringMode.SemiAutomatic || weapon.FiringMode == FiringMode.Meelee)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        weapon.Shoot();
                    }
                }

                if (Input.GetKeyDown(KeyCode.R) && weapon != null && !(weapon is Hands) && !(weapon is Grenade))
                {
                    weapon.Reload();
                }
            }
            else if (currentItem is Healable healingItem)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    healingItem.Use();
                }
            }
            else if (currentItem is Food foodItem)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    foodItem.Eat();
                }
            }
        }
    }

    public void SwitchItem()
    {
        if(isDragging)
        {
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && (!isInventoryOpened || !CraftingManager.Instance.isCraftingPanelOpened))
            {
                EquipItemFromInventory(i);
                ChangeSelectedSlot(i);
            }
        }
    }

    private void EquipItemFromInventory(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventorySlots.Count)
        {
            InventorySlot slot = inventorySlots[slotIndex];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                EquipItem(itemInSlot.item);
                ChangeSelectedSlot(slotIndex);
            }
        }
    }

    public void EquipFirstSlot()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                EquipItem(itemInSlot.item);
                ChangeSelectedSlot(i);
                return;
            }
        }
        EquipItem(hands);
    }
    #endregion
    #region ui/get method
    public void ChangeSelectedSlot(int newSlot)
    {
        int hotbarSlots = 9;

        if (newSlot < 0 || newSlot >= hotbarSlots)
            return;

        if (selectedSlot >= 0 && selectedSlot < hotbarSlots)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        InventoryItem itemInSlot = inventorySlots[newSlot].GetComponentInChildren<InventoryItem>();
        if(itemInSlot == null)
        {
            EquipItem(hands);
        }
        inventorySlots[newSlot].Select();
        selectedSlot = newSlot;
    }

    public void UpdateHotbar()
    {
        int temp = 0;
        for (int i = 0; i < 9; i++)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot.GetComponentInChildren<InventoryItem>() != null)
            {
                temp++;
            }

        }
        hotbarCount = temp;
    }

    private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpened = !isInventoryOpened;
            fullInventory.SetActive(isInventoryOpened);

            Cursor.lockState = isInventoryOpened ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isInventoryOpened;

            playerCam.enabled = !isInventoryOpened;
        }
    }

    public InventoryItem GetInventoryItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item)
            {
                return itemInSlot;
            }
        }
        return null;
    }

    public Item InstantiateItem(bool toWorld, bool toInventory, Item item)
    {
        if(toWorld && !toInventory)
        {
            Item itemToSpawn = Instantiate(item, PlayerManager.Instance.player.transform.position, Quaternion.identity);
            itemToSpawn.gameObject.SetActive(true);
            itemToSpawn.transform.SetParent(null);
            itemToSpawn.name = item.name;
            DropItemRigidBody(itemToSpawn);
            return itemToSpawn;
        }
        else if(!toWorld && toInventory)
        {
            Item itemToInventory = Instantiate(item, PlayerManager.Instance.player.transform.position, Quaternion.identity);
            itemToInventory.name = item.name;
            itemToInventory.GetComponent<ItemPickup>().Interact();
        }
        return null;
    }
    #endregion
}
// TODO
// inventory rework (95%):
// current slot selected -> moved item = currentitem (1 -> tab -> move item -> tab -> 2 -> drop item)
//                       -> moved item -> tab -> E -> drop item
//                       -> method for instantiating item (inventorymanager, craftingmanager, inventoryitem)
// stack system (95%) - current item fix (stacking 2nd slot, currently on 3rd slot)
// ~shift left mouse "split" inventory item
// crafting system (90%):
// shopping system (50%):
// money, items to sell, ui