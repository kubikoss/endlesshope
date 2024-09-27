using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AK47 : Weapon
{

    private void Start()
    {
        CurrentAmmo = MagazineSize;
    }

    public override void Shoot()
    {
        if (CurrentAmmo > 0)
        {
            RaycastHit hit;

            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, Range))
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

