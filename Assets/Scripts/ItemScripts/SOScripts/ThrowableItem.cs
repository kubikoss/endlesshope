using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Throwable", menuName = "ScriptableItem/Throwables")]
public class ThrowableItem : WeaponItem
{
    public int throwForce;
    public int explodeRadius;
}