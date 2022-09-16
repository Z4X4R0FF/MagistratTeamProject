using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlanetSettingsAsset : ScriptableObject
{
    [Header("Basic")] [Range(10, 200)] public float minRadius = 10;
    [Range(10, 200)] public float maxRadius = 200;

    public Color color;

    [Range(20, 256)] public int minPlanetResolution = 20;
    [Range(20, 256)] public int maxPlanetResolution = 20;

    [Header("NoiseSettings")] [Range(1, 10)]
    public int minNoiseLayersCount = 1;

    [Range(1, 10)] public int maxNoiseLayersCount = 10;

    [Range(1, 100)] public float minStrength = 1;
    [Range(1, 100)] public float maxStrength = 1;
    [Range(1, 100)] public float minBaseRoughness = 1;
    [Range(1, 100)] public float maxBaseRoughness = 1;
    [Range(1, 100)] public float minRoughness = 2;
    [Range(1, 100)] public float maxRoughness = 2;
    [Range(0, 100)] public float minPersistence = .5f;
    [Range(0, 100)] public float maxPersistence = .5f;
    public Vector3 centre;
    [Range(0, 100)] public float minMinValue;
    [Range(0, 100)] public float maxMinValue;
}