using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "ScriptableItem/Food")]
public class FoodItem : BaseItem
{
    public int foodStat;
    public bool isEatable = true;
}
