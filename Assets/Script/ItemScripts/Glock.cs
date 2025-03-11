using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Glock : Weapon
{
    public float baseSpread = 0.02f;
    public float movingSpread = 0.1f;
    public float maxSpread = 0.2f;
    public PlayerMovement playerMovement;
    [SerializeField]
    ParticleSystem particles;
    [SerializeField]
    AudioClip reloadSound;
    public Transform GSP;

    private void Start()
    {
        CurrentAmmo = MagazineSize;
        InitializeAmmoPool(weaponData.firingMode, weaponData.fullAmmo);
        CanShoot = true;
        IsReloading = false;

        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    public override void Shoot()
    {
        if (CurrentAmmo > 0 && CanShoot)
        {
            AudioManager.Instance.PlayAudio(ItemSound, 0.3f);
            Transform GSP = GameObject.Find("GSP").transform;

            ParticleSystem pts = ParticleManager.Instance.SpawnParticles(particles, GSP.position, 0.3f, true);
            pts.transform.SetParent(GSP);
            pts.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            RaycastHit hit;

            float playerSpeed = new Vector3(playerMovement.rb.velocity.x, 0f, playerMovement.rb.velocity.z).magnitude;
            float currentSpread = Mathf.Lerp(baseSpread, movingSpread, playerSpeed / playerMovement.moveSpeed);

            Vector3 spread = new Vector3(Random.Range(-currentSpread, currentSpread), Random.Range(-currentSpread, currentSpread), 0f);
            Vector3 shootDirection = PlayerManager.Instance.mainCamera.transform.forward + spread;

            if (Physics.Raycast(PlayerManager.Instance.mainCamera.transform.position, shootDirection, out hit, Range))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    PlayerAttack.Instance.AttackEnemy(hit.collider.gameObject, this);
                    ParticleManager.Instance.SpawnParticles(hit.collider.GetComponent<Enemy>().particles, hit.point, 0.3f);
                }   
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

        CanShoot = false;
        IsReloading = true;

        AudioManager.Instance.PlayAudio(reloadSound, 0.2f);

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
        CanShoot = true;
        IsReloading = false;
    }
}