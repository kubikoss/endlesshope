using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableItem")]
public class BaseItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
}