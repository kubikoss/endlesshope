using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    private Item currentItem;
    [SerializeField]
    private TextMeshProUGUI ammoText;
    [SerializeField]
    private TextMeshProUGUI fullAmmoText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        ammoText.gameObject.SetActive(true);
        Weapon.collectedWeapons.Clear();
        Weapon.ammoPools.Clear();
    }

    private void Update()
    {
        currentItem = InventoryManager.Instance.currentItem;
        UpdateAmmoText();
        ShowAmmo();
    }

    private void UpdateAmmoText()
    {
        if (currentItem is Weapon weapon && !(currentItem is Grenade) && weapon != null)
        {
            ammoText.gameObject.SetActive(true);
            if (weapon.TryGetComponent<AK47>(out var ak47))
            {
                ammoText.text = $"{weapon.CurrentAmmo}/{Weapon.ammoPools[ak47.weaponData.firingMode]}";
            }
            else if (weapon.TryGetComponent<Glock>(out var glock))
            {
                ammoText.text = $"{weapon.CurrentAmmo}/{Weapon.ammoPools[glock.weaponData.firingMode]}";
            }
        }
        else
        {
            ammoText.gameObject.SetActive(false);
        }
    }

    private void ShowAmmo()
    {
        if (InventoryManager.Instance.isInventoryOpened)
        {
            int ak47ammo = Weapon.ammoPools.ContainsKey(FiringMode.Automatic) ? Weapon.ammoPools[FiringMode.Automatic] : 0;
            int glockAmmo = Weapon.ammoPools.ContainsKey(FiringMode.SemiAutomatic) ? Weapon.ammoPools[FiringMode.SemiAutomatic] : 0;
            fullAmmoText.text = $"AR ammo: {ak47ammo} Glock Ammo: {glockAmmo}";
            fullAmmoText.gameObject.SetActive(true);
        }
        else
        {
            fullAmmoText.gameObject.SetActive(false);
        }
    }
}