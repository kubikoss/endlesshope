using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Weapon : Item
{
    public TextMeshProUGUI ammoText;
    public virtual int Damage => ((WeaponItem)itemData).damage;
    public virtual int MagazineSize => ((WeaponItem)itemData).magazineSize;
    public virtual float FireRate => ((WeaponItem)itemData).fireRate;
    public virtual int Range => ((WeaponItem)itemData).range;
    public virtual float ReloadSpeed => ((WeaponItem)itemData).reloadSpeed;
    public FiringMode FiringMode => ((WeaponItem)itemData).firingMode;
    private int currentAmmo;
    public int CurrentAmmo
    {
        get { return currentAmmo; }
        protected set { currentAmmo = value; }
    }

    private void Start()
    {
        ammoText.gameObject.SetActive(true);
    }

    public abstract void Shoot();
    public abstract void Reload();

    public void UpdateAmmoDisplay()
    {
        if(ammoText != null && InventoryManager.Instance.currentItem is Weapon)
        {
            if (ammoText.gameObject.activeSelf == false)
                ammoText.gameObject.gameObject.SetActive(true);

            ammoText.text = $"{CurrentAmmo}/{MagazineSize}";
        }
        /*else if(!(InventoryManager.Instance.currentItem is Weapon))
        {
            ammoText.gameObject.gameObject.SetActive(false);
        }*/
    }
}

public enum FiringMode
{
    SemiAutomatic,
    Automatic,
    Meelee
}