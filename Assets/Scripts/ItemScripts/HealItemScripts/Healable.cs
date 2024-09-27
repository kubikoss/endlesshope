using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Healable : Item
{
    [SerializeField]
    private int healAmmount;
    public int HealAmmount
    {
        get { return healAmmount; }
        protected set { healAmmount = value; }
    }

    public abstract void Use();
}
