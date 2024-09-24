using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon
{
    public Camera playerCamera;

    private void Start()
    {
        CurrentAmmo = MagazineSize;
    }

    public override void Shoot()
    {
        if (CurrentAmmo > 0)
        {
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

    public override void Reload()
    {
        if (CurrentAmmo < MagazineSize)
            CurrentAmmo = MagazineSize;
    }
}
