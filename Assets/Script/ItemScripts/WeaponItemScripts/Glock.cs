using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon
{
    public float baseSpread = 0.02f;
    public float movingSpread = 0.1f;
    public float maxSpread = 0.2f;
    public PlayerMovement playerMovement;

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