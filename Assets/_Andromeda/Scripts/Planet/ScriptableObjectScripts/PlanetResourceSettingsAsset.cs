using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlanetResourceSettingsAsset : ScriptableObject
{
    public ResourceType mainResource;
    public ResourceType secondaryResource;
    public ResourceType thirdResource;

    [Tooltip("MainResource takes x, secondary takes y of 1-x")] [Range(0, 1)]
    public float mainResourcePercentage;

    [Range(0, 1)] public float secondaryResourcePercentage;

    [Tooltip("Will be multiplied by planet radius/10")]
    public int minResourceSpawnCount;

    public int maxResourceSpawnCount;
}