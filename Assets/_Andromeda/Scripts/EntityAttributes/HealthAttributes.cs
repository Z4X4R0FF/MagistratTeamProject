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
    public int shield;
    public float shieldRegenerationPS;
    public int shieldRechargeDelay;
    public EntityTag entityTag;
    public EntityType entityType;
}

public enum EntityTag
{
    AIDamageable,
    PlayerDamageable
}

public enum EntityType
{
    Building,
    Ship
}