using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon
{
    private void Start()
    {
        CurrentAmmo = MagazineSize;
        InitializeAmmoPool(weaponData.name, weaponData.fullAmmo);
    }

    public override void Shoot()
    {
        if (CurrentAmmo > 0 && weaponData.canShoot)
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
        int neededAmmo = MagazineSize - CurrentAmmo;
        string weaponName = weaponData.name;

        if (Weapon.ammoPools[weaponName] <= 0)
        {
            yield break;
        }

        weaponData.canShoot = false;

        yield return new WaitForSeconds(ReloadSpeed);

        if (Weapon.ammoPools[weaponName] >= neededAmmo)
        {
            CurrentAmmo += neededAmmo;
            Weapon.ammoPools[weaponName] -= neededAmmo;
        }
        else
        {
            CurrentAmmo += Weapon.ammoPools[weaponName];
            Weapon.ammoPools[weaponName] = 0;
        }
        weaponData.canShoot = true;
    }
}