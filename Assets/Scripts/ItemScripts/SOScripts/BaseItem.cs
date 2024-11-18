using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableItem")]
public class BaseItem : ScriptableObject
{
    public string itemName;
    public int ID;
    public Sprite itemIcon;
    public int maxStackCount;
    public bool isStackable = false;
    public GameObject itemWorld;
}