using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    private readonly Noise _noise = new();
    private readonly NoiseSettings.RigidNoiseSettings _noiseSettings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings noiseSettings)
    {
        _noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;

        var amplitude = 1f;
        var weight = 1f;
        var frequency = _noiseSettings.baseRoughness;

        for (var i = 0; i < _noiseSettings.numLayers; i++)
        {
            var v = 1 - Mathf.Abs(_noise.Evaluate(point * frequency + _noiseSettings.centre));
            v *= v;
            v *= weight;
            weight = v * Mathf.Clamp01(_noiseSettings.weightMultiplier);
            noiseValue += v * amplitude;
            frequency *= _noiseSettings.roughness;
            amplitude *= _noiseSettings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _noiseSettings.minValue);
        return noiseValue * _noiseSettings.strength;
    }
}