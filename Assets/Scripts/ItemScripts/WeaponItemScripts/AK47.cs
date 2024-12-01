using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EventSystems;
using UnityEngine.EventSystems;

public class AK47 : Weapon
{
    public float baseSpread = 0.02f;
    public float movingSpread = 0.1f;
    public float maxSpread = 0.2f;
    public PlayerMovement playerMovement;

    private void Start()
    {
        CurrentAmmo = MagazineSize;
        if (playerMovement == null)
            playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateAmmoDisplay();
    }

    public override void Shoot()
    {
        var weaponItem = (WeaponItem)itemData;

        if (CurrentAmmo > 0 && weaponItem.canShoot)
        {
            RaycastHit hit;

            float playerSpeed = new Vector3(playerMovement.rb.velocity.x, 0f, playerMovement.rb.velocity.z).magnitude;
            float currentSpread = Mathf.Lerp(baseSpread, movingSpread, playerSpeed / playerMovement.moveSpeed);

            Vector3 spread = new Vector3(Random.Range(-currentSpread, currentSpread), Random.Range(-currentSpread, currentSpread), 0f);

            Vector3 shootDirection = PlayerCamera.transform.forward + spread;

            if (Physics.Raycast(PlayerCamera.transform.position, shootDirection, out hit, Range))
            {

                if (hit.collider.CompareTag("Enemy"))
                    PlayerAttack.Instance.AttackEnemy(hit.collider.gameObject, this);
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