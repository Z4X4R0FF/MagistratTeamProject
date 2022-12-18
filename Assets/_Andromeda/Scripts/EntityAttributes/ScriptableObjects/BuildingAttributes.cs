using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingAttributes : ScriptableObject
{
    public BuildingType type;
    public Sprite icon;
    public int metalCost;
    public int uraniumPerTickCost;
    public HealthAttributes healthAttributes;
    public GameObject prefab;
    [ConditionalHide("type", 1)] public WeaponAttributes weaponAttributes;
    [ConditionalHide("type", 1)] public EntityTag enemyTag;
    [ConditionalHide("type", 0)] public HarvestAttributes harvestAttributes;
    [ConditionalHide("type", 3)] public HousingAttributes housingAttributes;
}

public enum BuildingType
{
    Harvest,
    Attack,
    Defence,
    Housing,
}

[Serializable]
public class HousingAttributes
{
    public int capacity;
    public int mealsPerTickCost;
}

[Serializable]
public class HarvestAttributes
{
    public ResourceType harvestResourceType;
    public int harvestAmount;
}