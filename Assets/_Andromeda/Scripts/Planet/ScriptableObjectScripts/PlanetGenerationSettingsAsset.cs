using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlanetGenerationSettingsAsset : ScriptableObject
{
    [Header("Basic")] [Range(10, 200)] public int minRadius = 10;
    [Range(10, 200)] public int maxRadius = 200;

    [Range(20, 256)] public int planetResolution = 20;

    [Header("NoiseSettings")] [SerializeField]
    public List<NoiseLayerSettingsAsset> noiseLayers;

    [SerializeField] public PlanetColorSettingsAsset colorSettings;

    private void OnValidate()
    {
        Validated?.Invoke();
    }

    public event Action Validated;
}