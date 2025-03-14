using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
        if (currentItem is Weapon weapon && !(currentItem is Grenade))
        {
            ammoText.gameObject.SetActive(true);
            if(weapon is AK47)
            {
                ammoText.text = $"{weapon.CurrentAmmo}/{Weapon.ammoPools[weapon.GetComponent<AK47>().weaponData.firingMode]}";
            }
            else if(weapon is Glock)
            {
                ammoText.text = $"{weapon.CurrentAmmo}/{Weapon.ammoPools[weapon.GetComponent<Glock>().weaponData.firingMode]}";
            }
        }
        else
        {
            ammoText.gameObject.SetActive(false);
        }
    }

    private void ShowAmmo()
    {
        if(InventoryManager.Instance.isInventoryOpened)
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