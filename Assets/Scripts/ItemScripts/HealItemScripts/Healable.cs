using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healable : Item
{
    public int HealAmount => ((HealableItem)itemData).healAmount;

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