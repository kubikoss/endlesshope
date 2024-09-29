using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Food : Item
{
    public FoodItem foodItemData;

    public void Eat()
    {
        Player.Instance.UpdateHunger(this);
        Destroy(gameObject);
    }
}
