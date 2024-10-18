using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableItem")]
public class BaseItem : ScriptableObject
{
    [Header("Base Item")]
    public string itemName;
    public int ID;
    public Sprite itemIcon;
    public bool isStackable = false;

    [Header("Prefab")]
    public GameObject handItem;
}