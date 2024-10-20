using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Weapon
{
    public int throwForce => ((ThrowableItem)itemData).throwForce;
    private int explodeRadius => ((ThrowableItem)itemData).explodeRadius;
    public bool IsBeingThrown { get; private set; }

    Rigidbody rb;
    InventoryItem inventoryItem;

    private void Start()
    {
        CurrentAmmo = 1;
        IsBeingThrown = false;
    }

    public override void Shoot()
    {
        ThrowGrenade();
    }

    public override void Reload()
    {

    }

    private bool ThrowGrenade()
    {
        if (CurrentAmmo > 0)
        {
            CurrentAmmo--;
            IsBeingThrown = true;

            transform.SetParent(null);

            rb = gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Collider>().isTrigger = false;
            rb.AddForce(PlayerCamera.transform.forward * throwForce, ForceMode.Impulse);

            //UpdateInventoryItem();
            InventoryManager.Instance.RemoveItem(this);
            InventoryManager.Instance.EquipFirstSlot();
            InventoryManager.Instance.ChangeSelectedSlot(0);

            StartCoroutine(ExplodeGrenade(this));

            return true;
        }

        return false;
    }

    private IEnumerator ExplodeGrenade(Grenade grenade)
    {
        yield return new WaitForSeconds(2f);

        Collider[] explosionColliders = Physics.OverlapSphere(grenade.transform.position, explodeRadius);
        foreach (var explosionCollider in explosionColliders)
        {
            Enemy enemy = explosionCollider.GetComponent<Enemy>();
            Player player = explosionCollider.GetComponent<Player>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }
            else if (player != null)
            {
                player.TakeDamage(Damage / Random.Range(3,5));
            }
            else
            {
                Debug.Log(explosionCollider.name);
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explodeRadius);
    }
}