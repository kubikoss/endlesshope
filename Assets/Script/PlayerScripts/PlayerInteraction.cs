using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private float interactionRange = 2f;
    [SerializeField]
    private float pickupCooldown = 0.2f;
    private bool canPickup = true;

    private ShopItem currentlShopItem;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && canPickup)
        {
            StartCoroutine(PickupItem());
        }
        CheckCollisionLook();
    }

    private IEnumerator PickupItem()
    {
        canPickup = false;

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
        yield return new WaitForSeconds(pickupCooldown);
        canPickup = true;
    }

    private void CheckCollisionLook()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            ShopItem shop = hit.collider.GetComponent<ShopItem>();
            if (shop != null)
            {
                if (currentlShopItem != null && currentlShopItem != shop)
                {
                    currentlShopItem.isLooking = false;
                }
                shop.isLooking = true;
                currentlShopItem = shop;
            }
            return;
        }

        if (currentlShopItem != null)
        {
            currentlShopItem.isLooking = false;
            currentlShopItem = null;
        }
    }
}