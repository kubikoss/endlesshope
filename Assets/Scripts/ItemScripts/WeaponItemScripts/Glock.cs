using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon
{
    private void Start()
    {
        CurrentAmmo = MagazineSize;
    }

    private void Update()
    {
        UpdateAmmoDisplay(CurrentAmmo);
    }

    public override void Shoot()
    {
        var weaponItem = (WeaponItem)itemData;

        if (CurrentAmmo > 0 && weaponItem.canShoot)
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
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        var weaponItem = (WeaponItem)itemData;
        weaponItem.canShoot = false;

        yield return new WaitForSeconds(ReloadSpeed);

        CurrentAmmo = MagazineSize;
        weaponItem.canShoot = true;
    }
}