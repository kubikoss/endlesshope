using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory")]
    public List<InventorySlot> inventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject fullInventory;
    [HideInInspector] public bool isInventoryOpened = false;
    [HideInInspector] public bool isDragging = false;
    [HideInInspector] public int selectedSlot = -1;
    [HideInInspector] public int hotbarCount = 0;
    [HideInInspector] public bool splitting = false;
    [SerializeField]
    private GameObject endArrow;

    [Header("Item")]
    public Item currentItem;
    private float fireTimer = 0f;

    [Header("Camera")]
    public Camera playerCam;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
        playerCam = PlayerManager.Instance.mainCamera;
        endArrow.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(!MenuManager.Instance.isPaused)
        {
            if (SleepManager.Instance.isSleeping || Player.Instance.endPanel.activeSelf)
                return;

            SwitchItem();
            HandleCurrentItem();
            OpenInventory();
            DropItem(currentItem);
        }

        CheckForCure();
    }

    #region add/remove from inventory
    public int AddItem(Item item)
    {
        if(isDragging || Player.Instance.endPanel.activeSelf || SleepManager.Instance.isSleeping || MenuManager.Instance.isPaused)
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
                        AudioManager.Instance.DropItemAudio();
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
            if(currentItem != null)
                Player.Instance.GetComponent<Animator>().SetBool("isNull", true);
            else
                Player.Instance.GetComponent<Animator>().SetBool("isNull", false);

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
            else if (currentItem is Bed bed && !SleepManager.Instance.isSleeping)
            {
                HandleBed(bed);
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

    private void HandleBed(Bed bed)
    {
        if(Input.GetMouseButtonDown(0))
        {
            bed.Sleep();
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

            PlayerMovement.Instance.canRotateCam = !isInventoryOpened;

            if (!isInventoryOpened)
            {
                CraftingManager.Instance.TryPutItemsBackInInventory();
            }
            CraftingManager.Instance.CheckCraftingSlotsForRecipes();
        }
    }

    private void CheckForCure()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item != null)
            {
                if (itemInSlot.item.ItemName == "Cure")
                {
                    endArrow.gameObject.SetActive(true);
                    return;
                }
            }
        }
        endArrow.gameObject.SetActive(false);
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
//game-other (99.7%)

//OTHER TODO
//zombies spawn

//finished 99%