using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Weapon
{
    [SerializeField]
    private float throwForce = 10f;
    [SerializeField]
    private float explosionRadius = 5f;

    //private Camera playerCamera;
    Rigidbody rb;

    private void Start()
    {
        CurrentAmmo = 1;
        //playerCamera = Camera.main;
        
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

        Collider[] explosionColliders = Physics.OverlapSphere(grenade.transform.position, explosionRadius);
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
