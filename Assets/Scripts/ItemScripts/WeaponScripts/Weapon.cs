using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FiringMode
{
    SemiAutomatic,
    Automatic,
    Meelee
}
public abstract class Weapon : Item
{
    [SerializeField]
    private int damage;
    public int Damage
    {
        get { return damage; }
        protected set { damage = value; }
    }

    [SerializeField]
    private int magazineSize;
    public int MagazineSize
    {
        get { return magazineSize; }
        protected set { magazineSize = value; }
    }

    [SerializeField]
    private float fireRate;
    public float FireRate
    {
        get { return fireRate; }
        protected set { fireRate = value; }
    }

    [SerializeField]
    private float range;
    public float Range
    {
        get { return range; }
        protected set { range = value; }
    }

    [SerializeField]
    private float reloadSpeed;
    public float ReloadSpeed
    {
        get { return reloadSpeed; }
        protected set { reloadSpeed = value; }
    }

    [SerializeField]
    private FiringMode firingMode;

    public FiringMode FiringMode
    {
        get { return firingMode; }
        protected set { firingMode = value; }
    }

    private int currentAmmo;
    public int CurrentAmmo
    {
        get { return currentAmmo; }
        protected set { currentAmmo = value; }
    }

    public abstract void Shoot();
    public abstract void Reload();
}
