using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : Weapon
{
    public Camera playerCamera;

    private void Start()
    {
        CurrentAmmo = 1;
        FiringMode = FiringMode.Meelee;
    }

    public override void Shoot()
    {
        MeeleeAttack();
    }

    public override void Reload()
    {

    }

    private void MeeleeAttack()
    {
        CurrentAmmo = 1;
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Range))
        {
            if (hit.collider.CompareTag("Enemy"))
                PlayerAttack.Instance.AttackEnemy(hit.collider.gameObject, this);
            else
                Debug.Log(hit.collider);
        }
        CurrentAmmo--;
        Debug.Log(CurrentAmmo);
    }
}
