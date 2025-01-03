using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    private Item currentItem;
    [SerializeField]
    TextMeshProUGUI ammoText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        ammoText.gameObject.SetActive(true);
    }

    private void Update()
    {
        currentItem = InventoryManager.Instance.currentItem;
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        if (currentItem is Weapon weapon && !(currentItem is Grenade))
        {
            ammoText.gameObject.SetActive(true);
            ammoText.text = $"{weapon.CurrentAmmo}/{Weapon.ammoPools[weapon.name]}";
        }
        else
        {
            ammoText.gameObject.SetActive(false);
        }
    }
}