using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : Weapon
{
    private void Start()
    {
        CurrentAmmo = 1;
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

        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, Range))
        {
            if (hit.collider.CompareTag("Enemy"))
                PlayerAttack.Instance.AttackEnemy(hit.collider.gameObject, this);
        }
        CurrentAmmo--;
        Debug.Log(CurrentAmmo);
    }
}