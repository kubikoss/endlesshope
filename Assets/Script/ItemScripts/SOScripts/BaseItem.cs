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
    public bool isStackable = false;
    public int maxStackCount;
    public GameObject itemWorld;
    public float rotX;
    public float rotY;
    public float rotZ;
}