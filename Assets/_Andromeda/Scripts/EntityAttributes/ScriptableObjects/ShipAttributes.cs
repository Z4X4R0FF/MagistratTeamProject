using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShipAttributes : ScriptableObject
{
    public HealthAttributes healthAttributes;
    public MovementAttributes movementAttributes;
    public WeaponAttributes weaponAttributes;
    public EntityTag enemyTag;
}
