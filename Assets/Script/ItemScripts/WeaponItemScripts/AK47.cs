﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;

public class AK47 : Weapon
{
    public float baseSpread;
    public float movingSpread;
    public PlayerMovement playerMovement;
    [SerializeField]
    ParticleSystem particles;

    private void Start()
    {
        CurrentAmmo = MagazineSize;
        InitializeAmmoPool(weaponData.firingMode, weaponData.fullAmmo);

        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    public override void Shoot()
    {
        if (CurrentAmmo > 0 && weaponData.canShoot)
        {
            
            RaycastHit hit;

            float playerSpeed = new Vector3(playerMovement.rb.velocity.x, 0f, playerMovement.rb.velocity.z).magnitude;
            float currentSpread = Mathf.Lerp(baseSpread, movingSpread, playerSpeed / playerMovement.moveSpeed);

            Vector3 spread = new Vector3(Random.Range(-currentSpread, currentSpread), Random.Range(-currentSpread, currentSpread), 0f);
            Vector3 shootDirection = PlayerManager.Instance.mainCamera.transform.forward + spread;

            if (Physics.Raycast(PlayerManager.Instance.mainCamera.transform.position, shootDirection, out hit, Range))
            {
                if (hit.collider.CompareTag("Enemy"))
                    PlayerAttack.Instance.AttackEnemy(hit.collider.gameObject, this);
            }
            
            CurrentAmmo--;
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
        FiringMode firingMode = weaponData.firingMode;

        if (Weapon.ammoPools[firingMode] <= 0)
        {
            yield break;
        }

        weaponData.canShoot = false;

        yield return new WaitForSeconds(ReloadSpeed);

        if (Weapon.ammoPools[firingMode] >= neededAmmo)
        {
            CurrentAmmo += neededAmmo;
            Weapon.ammoPools[firingMode] -= neededAmmo;
        }
        else
        {
            CurrentAmmo += Weapon.ammoPools[firingMode];
            Weapon.ammoPools[firingMode] = 0;
        }
        weaponData.canShoot = true;
    }
}