using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Weapon
{
    public int throwForce => ((ThrowableItem)itemData).throwForce;
    private int explodeRadius => ((ThrowableItem)itemData).explodeRadius;

    Rigidbody rb;

    private void Start()
    {
        CurrentAmmo = 1;
    }

    public override void Shoot()
    {
        ThrowGrenade();
    }

    public override void Reload()
    {

    }

    private void ThrowGrenade()
    {
        if (CurrentAmmo > 0)
        {
            CurrentAmmo--;

            transform.SetParent(null);

            rb = gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Collider>().isTrigger = false;
            rb.AddForce(PlayerCamera.transform.forward * throwForce, ForceMode.Impulse);

            InventoryManager.Instance.RemoveItem(this);
            //InventoryManager.Instance.EquipFirstSlot();

            StartCoroutine(ExplodeGrenade(this));
        }
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
        InventoryManager.Instance.EquipFirstSlot();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explodeRadius);
    }
}