using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableItem/Weapon")]
public class WeaponItem : BaseItem
{
    public int damage;
    public int fireRate;
    public int magazineSize;
    public int range;
    public float reloadSpeed;
    public FiringMode firingMode;
}