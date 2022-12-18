using Assets.Scripts.Vehicles.Rover;
using UnityEngine;

[CreateAssetMenu]
public class RoverAttributes : ScriptableObject
{
    public Rover prefab;
    public HealthAttributes healthAttributes;
    public PlayerRoverMovementAttributes playerRoverMovementAttributes;
    public WeaponAttributes weaponAttributes;
}