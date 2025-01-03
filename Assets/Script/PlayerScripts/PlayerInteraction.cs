using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public Camera playerCamera;

    public float pickupCooldown = 0.2f;
    private bool canPickup = true;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && canPickup)
        {
            StartCoroutine(PickupItem()); 
        }
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
}