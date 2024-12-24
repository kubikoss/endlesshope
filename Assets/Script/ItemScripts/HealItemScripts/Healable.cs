using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healable : Item
{
    public HealableItem healableData;
    public int HealAmount => healableData.healAmount;

    public void Use()
    {
        HealPlayer();
    }

    public void HealPlayer()
    {
        Player.Instance.Heal(HealAmount);
        UpdateInventoryItemCountOnUse();
    }
}