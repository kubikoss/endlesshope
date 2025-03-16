using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Weapon
{
    public ThrowableItem throwableItem;
    public int ThrowForce => throwableItem.throwForce;
    private int ExplodeRadius => throwableItem.explodeRadius;
    public bool IsBeingThrown { get; private set; }

    Rigidbody rb;
    InventoryItem inventoryItem;

    private void Start()
    {
        IsBeingThrown = false;
    }

    public override void Shoot()
    {
        ThrowGrenade();
    }

    public override void Reload()
    {

    }

    public bool ThrowGrenade()
    {
        IsBeingThrown = true;

        transform.SetParent(null);
        rb = gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Collider>().isTrigger = false;
        rb.AddForce(PlayerManager.Instance.mainCamera.transform.forward * ThrowForce, ForceMode.Impulse);

        InventoryItem invItem = InventoryManager.Instance.GetInventoryItem(this);
        invItem.RemoveItemFromInventory();
        InventoryManager.Instance.EquipHands();
        StartCoroutine(ExplodeGrenade(this));
        return true;
    }


    private IEnumerator ExplodeGrenade(Grenade grenade)
    {
        yield return new WaitForSeconds(3f);

        AudioManager.Instance.PlayAudio(ItemSound);

        Collider[] explosionColliders = Physics.OverlapSphere(grenade.transform.position, ExplodeRadius);
        foreach (var explosionCollider in explosionColliders)
        {
            Enemy enemy = explosionCollider.GetComponent<Enemy>();
            Player player = explosionCollider.GetComponent<Player>();
            AllyController ally = explosionCollider.GetComponent<AllyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }
            else if (player != null)
            {
                player.TakeDamage(Damage / Random.Range(3,5));
            }
            else if(ally != null)
            {
                ally.AllyTakeDamage(Damage / Random.Range(3, 5));
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, ExplodeRadius);
    }
}