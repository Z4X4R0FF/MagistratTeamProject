using System;
using UnityEngine;

[Serializable]
public class PlayerStarshipMovementAttributes
{
    [Header("Starship movement")]
    public float startSpeed;
    public float minSpeed;
    public float maxSpeed;
    public float accelerateSpeed;
    public float slowDownSpeed;
    public float rotationSpeed;
}