using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Weapon : Item
{
    public WeaponItem weaponData;

    // ammo pools
    public static Dictionary<string, int> ammoPools = new Dictionary<string, int>();
    public static HashSet<Item> collectedWeapons = new HashSet<Item>();

    public virtual int Damage => weaponData.damage;
    public virtual int MagazineSize => weaponData.magazineSize;
    public virtual float FireRate => weaponData.fireRate;
    public virtual int Range => weaponData.range;
    public virtual float ReloadSpeed => weaponData.reloadSpeed;
    public FiringMode FiringMode => weaponData.firingMode;
    private int currentAmmo;
    public int CurrentAmmo
    {
        get { return currentAmmo; }
        protected set { currentAmmo = value; }
    }

    protected void InitializeAmmoPool(string weaponName, int fullAmmo)
    {
        if(!ammoPools.ContainsKey(weaponName))
        {
            ammoPools[weaponName] = fullAmmo;
        }
    }

    public abstract void Shoot();
    public abstract void Reload();
}

public enum FiringMode
{
    SemiAutomatic,
    Automatic
}