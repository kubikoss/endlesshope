using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Healable : Item
{
    [SerializeField]
    private int healAmount;
    public int HealAmount
    {
        get { return healAmount; }
        protected set { healAmount = value; }
    }

    public abstract void Use();
}
