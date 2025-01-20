using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
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
    [HideInInspector] public int selectedSlot = -1;
    [HideInInspector] public int hotbarCount = 0;
    [HideInInspector] public bool splitting = false;

    [Header("Item")]
    public Item currentItem;
    private float fireTimer = 0f;

    [Header("Camera")]
    public PlayerCam playerCam;

    public ParticleSystem particles;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerCam = PlayerManager.Instance.mainCamera;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        SwitchItem();
        HandleCurrentItem();
        OpenInventory();
        DropItem(currentItem);
    }

    #region add/remove from inventory
    public int AddItem(Item item)
    {
        if(isDragging)
        {
            return -1;
        }
        if(item.GetComponent<Grenade>() != null && item.GetComponent<Grenade>().IsBeingThrown)
        {
            return -1;
        }

        // Stacking item
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item.ID == item.ID && itemInSlot.count < item.MaxStackCount && !splitting)
            {
                if (itemInSlot.item.IsStackable && item.IsStackable)
                {
                    itemInSlot.count++;
                    itemInSlot.UpdateCount();
                    Destroy(item.gameObject);
                    return 2; // Item stacked and not equipped
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

    private void DropItem(Item itemToDrop)
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            if (!(itemToDrop is Hands) && itemToDrop != null)
            {
                for (int i = 0; i < inventorySlots.Count; i++)
                {
                    InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

                    if (itemInSlot != null && itemInSlot.item == itemToDrop)
                    {
                        // Dropping single item
                        if (itemInSlot.count == 1)
                        {
                            itemInSlot.RemoveItemFromInventory();
                            itemToDrop.transform.SetParent(null);
                            currentItem = null;
                            EquipHands();
                            hotbarCount--;
                            DropItemRigidBody(itemToDrop);
                        }
                        // Dropping stacked item
                        else if (itemInSlot.count > 1)
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
    }

    public void DropItemRigidBody(Item item)
    {
        Vector3 cameraForward = playerCam.transform.forward;
        cameraForward.y = 2f;
        cameraForward.Normalize();

        if(item.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = item.AddComponent<Rigidbody>();
            rb.AddForce(cameraForward * 5f, ForceMode.Impulse);
        }
        if(item.GetComponent<Collider>() != null)
        {
            item.GetComponent<Collider>().isTrigger = false;
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

    public Item InstantiateItem(bool toWorld, bool toInventory, Item item)
    {
        if (toWorld && !toInventory)
        {
            Item itemToSpawn = Instantiate(item, PlayerManager.Instance.player.transform.position, Quaternion.identity);
            itemToSpawn.gameObject.SetActive(true);
            itemToSpawn.transform.SetParent(null);
            itemToSpawn.name = item.name;
            DropItemRigidBody(itemToSpawn);
            return itemToSpawn;
        }
        else if (!toWorld && toInventory)
        {
            Item itemToInventory = Instantiate(item, PlayerManager.Instance.player.transform.position, Quaternion.identity);
            itemToInventory.name = item.name;
            itemToInventory.GetComponent<ItemPickup>().Interact();
        }
        return null;
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

    private void SwitchItem()
    {
        if(isDragging || currentItem is Weapon weapon && weapon.IsReloading)
        {
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && !isInventoryOpened)
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
            else
            {
                EquipHands();
            }
        }
        else
        {
            EquipHands();
        }
    }

    private void ChangeSelectedSlot(int newSlot)
    {
        int hotbarSlots = 9;

        if (newSlot < 0 || newSlot >= hotbarSlots)
            return;

        if (selectedSlot >= 0 && selectedSlot < hotbarSlots)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newSlot].Select();
        selectedSlot = newSlot;
    }

    public void EquipHands()
    {
        if (currentItem != null && currentItem.gameObject.activeSelf)
        {
            if (!(currentItem is Grenade grenade) || !grenade.IsBeingThrown)
            {
                currentItem.gameObject.SetActive(false);
            }
        }
        currentItem = null;
    }
    #endregion
    #region handling current item
    private void HandleCurrentItem()
    {
        if (!isInventoryOpened && !isDragging)
        {
            if (currentItem == null)
            {
                PlayerAttack.Instance.DefaultAttack();
            }
            else if (currentItem is Weapon weapon)
            {
                HandleWeapon(weapon);
            }
            else if (currentItem is Healable healable)
            {
                if (Player.Instance.hp < 100)
                {
                    HandleHealable(healable);
                }
            }
            else if (currentItem is Food food)
            {
                HandleFood(food);
            }
        }
    }

    private void HandleWeapon(Weapon weapon)
    {
        fireTimer += Time.deltaTime;

        if (weapon.FiringMode == FiringMode.Automatic)
        {
            if (Input.GetMouseButton(0) && fireTimer >= (1f / weapon.FireRate))
            {
                weapon.Shoot();
                fireTimer = 0f;
                ParticleSystem pts = ParticleManager.Instance.SpawnParticles(particles, currentItem.gameObject.GetComponentInChildren<Transform>().position, 0.3f);
                pts.transform.SetParent(currentItem.gameObject.GetComponentInChildren<Transform>());
            }
        }
        else if (weapon.FiringMode == FiringMode.SemiAutomatic)
        {
            if (Input.GetMouseButtonDown(0))
            {
                weapon.Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && weapon != null)
        {
            if (!(weapon is Hands) && !(weapon is Grenade) && !weapon.IsReloading)
            {
                weapon.Reload();
            }
        }
    }

    private void HandleHealable(Healable healable)
    {
        if (Input.GetMouseButtonDown(0))
        {
            healable.Use();
        }
    }

    private void HandleFood(Food food)
    {
        if (Input.GetMouseButtonDown(0))
        {
            food.Eat();
        }
    }
    #endregion
    #region ui/get method
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
        if(Input.GetKeyDown(KeyCode.Tab) && !isDragging)
        {
            isInventoryOpened = !isInventoryOpened;
            fullInventory.SetActive(isInventoryOpened);

            Cursor.lockState = isInventoryOpened ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isInventoryOpened;

            playerCam.enabled = !isInventoryOpened;

            if (!isInventoryOpened)
            {
                CraftingManager.Instance.TryPutItemsBackInInventory();
            }
            CraftingManager.Instance.CheckCraftingSlotsForRecipes();
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
    #endregion
}
// inventory rework (100%)
// crafting system (99%)
// shopping system (97%)
// ally system (97%)
// models (75%)  
// game-other (86%)

//project TODO

//CODE TODO
//minimap-> 1 day
//tutorial-> 1 day
//chest-> this week
//shopitem rework - 90%

//MODELS TODO
//map & item models - town 95%, farm 50%, airport 90%, military, graveyard, port 95%
//resources - bag of sand, metal piece, glass, wood, plastic, cork, rubber, bed, chest
//vfx & sfx & animations & ui (menu,.., achievements) (animations this week)

//OTHER TODO
//sleep bar fatigue effects
//crafting recipes - after models

//finished 85%