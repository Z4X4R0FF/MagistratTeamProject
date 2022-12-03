using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Vehicles.Starship;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class ShipAttributes : ScriptableObject
{
    public ShipType shipType;
    public HealthAttributes healthAttributes;
    [ConditionalHide("shipType", 0)] public StarshipData starshipData;
    [FormerlySerializedAs("movementAttributes")] [ConditionalHide("shipType", 1)] public AiMovementAttributes aiMovementAttributes;
    public WeaponAttributes weaponAttributes;
    public EntityTag enemyTag;
}

public enum ShipType
{
    Player,
    AI
}