using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HealthAttributes
{
    public int health;
    public int protection;
    public float regenerationPS;
    public EntityTag entityTag;
}

public enum EntityTag
{
    AIDamageable,
    PlayerDamageable
}