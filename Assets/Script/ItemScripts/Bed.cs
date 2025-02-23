using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Item
{
    public BedItem bedItemData;
    public int SleepAmount => bedItemData.sleepAmount;

    public void Sleep()
    {
        Player.Instance.Sleep(SleepAmount);
        UpdateInventoryItemCountOnUse();
    }
}