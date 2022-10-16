using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseLayerSettingsAsset : ScriptableObject
{
    public bool isEnabled;
    public bool useFirstLayerAsMask;
    public NoiseSettings noiseSettings;

    private void OnValidate()
    {
        Validated?.Invoke();
    }

    public event Action Validated;
}

[System.Serializable]
public class NoiseSettings
{
    public FilterType filterType;

    [ConditionalHide("filterType", 0)] public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)] public RigidNoiseSettings rigidNoiseSettings;

    public enum FilterType
    {
        Simple,
        Rigid
    }

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public bool useRandomCentre;
        [Range(0, 10)] public float strength = 0;
        [Range(1, 10)] public int numLayers = 1;
        [Range(0, 10)] public float baseRoughness = 1;
        [Range(0, 10)] public float roughness = 1;
        [Range(0, 10)] public float persistence = .5f;
        public Vector3 centre;
        [Range(0, 100)] public float minValue;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = .8f;
    }
}