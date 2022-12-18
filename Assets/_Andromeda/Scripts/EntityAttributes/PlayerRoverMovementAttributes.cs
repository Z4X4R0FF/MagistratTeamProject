using System;
using UnityEngine;

[Serializable]
public class PlayerRoverMovementAttributes
{
    [Header("Rover movement")]
    public float maxSpeed;
    public float accelerateSpeed;
    public float slowDownSpeed;
    public float rotationSpeed;
}