using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Food : Item
{
    public FoodItem foodItemData;

    public void Eat()
    {
        AudioManager.Instance.EatItemAudio();
        Player.Instance.UpdateHunger(this);
        UpdateInventoryItemCountOnUse();
    }
}