using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Money", menuName = "ScriptableItem/Currency")]
public class MoneyItem : BaseItem
{
    public int amount;
    public int maxAmount;
}