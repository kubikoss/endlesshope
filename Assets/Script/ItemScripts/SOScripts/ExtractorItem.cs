using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Venom Extractor", menuName = "ScriptableItem/Extractor")]
public class ExtractorItem : BaseItem
{
    public float range;
    public int ammo;
    public bool canTakeVenom;
    public float reloadTime;
}