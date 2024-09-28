using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Healable : Item
{
    public virtual int HealAmount => ((HealableItem)itemData).healAmount;

    public abstract void Use();
}
