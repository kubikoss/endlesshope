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
        rb = gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Collider>().isTrigger = false;

        if (CurrentAmmo > 0)
        {
            rb.AddForce(PlayerCamera.transform.forward * throwForce, ForceMode.Impulse);
            CurrentAmmo--;
            
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
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }
            else
            {
                Debug.Log(explosionCollider.name);
            }
        }
        Destroy(gameObject);
        InventoryManager.Instance.EquipFirstSlot();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explodeRadius);
    }
}
