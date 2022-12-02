using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponAttributes
{
    public int damage;
    public bool ignoresProtection;
    public WeaponType weaponType;
    public float shotDelay;
    public Color weaponColor;
}

public enum WeaponType
{
    Laser
}