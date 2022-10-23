using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlanetGenerationSettingsAsset : ScriptableObject
{
    [Header("Basic")] [Range(50, 500)] public int minRadius = 100;
    [Range(100, 500)] public int maxRadius = 100;

    [Range(0.5f, 10)] public float planetResolutionScaleFactor = 1f;

    [Tooltip("will count as elevationMinMax.Max - Min Lerp")] [Range(0, 1)]
    public float spawnHeightPercentage;

    [Header("NoiseSettings")] [SerializeField]
    public List<NoiseLayerSettingsAsset> noiseLayers;

    [SerializeField] public PlanetColorSettingsAsset colorSettings;
    [SerializeField] public PlanetResourceSettingsAsset resourceSettings;
    [SerializeField] public PlanetPropsSettingsAsset propSettings;
    [SerializeField] public MobRaceAsset mobAsset;
    private void OnValidate()
    {
        Validated?.Invoke();
    }

    public event Action Validated;
}