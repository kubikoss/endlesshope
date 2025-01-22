using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public abstract class Weapon : Item
{
    public WeaponItem weaponData;

    // Ammo pools
    public static Dictionary<FiringMode, int> ammoPools = new Dictionary<FiringMode, int>();
    public static HashSet<Item> collectedWeapons = new HashSet<Item>();

    public virtual int Damage => weaponData.damage;
    public virtual int MagazineSize => weaponData.magazineSize;
    public virtual float FireRate => weaponData.fireRate;
    public virtual int Range => weaponData.range;
    public virtual float ReloadSpeed => weaponData.reloadSpeed;
    public bool CanShoot
    {
        get { return weaponData.canShoot; }
        protected set { weaponData.canShoot = value; }
    }
    public bool IsReloading
    {
        get { return weaponData.isReloading; }
        set { weaponData.isReloading = value; }
    }
    public FiringMode FiringMode => weaponData.firingMode;
    private int currentAmmo;
    public int CurrentAmmo
    {
        get { return currentAmmo; }
        protected set { currentAmmo = value; }
    }
    public AudioClip ItemSound
    {
        get { return weaponData.itemSound; }
        set { weaponData.itemSound = value; }
    }
    
    protected void InitializeAmmoPool(FiringMode firingmode, int fullAmmo)
    {
        if(!ammoPools.ContainsKey(firingmode))
        {
            ammoPools[firingmode] = fullAmmo;
        }
        CanShoot = true;
        IsReloading = false;
    }

    public abstract void Shoot();
    public abstract void Reload();
}

public enum FiringMode
{
    SemiAutomatic,
    Automatic
}