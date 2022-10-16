using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleNoiseFilter : INoiseFilter
{
    private readonly Noise _noise = new();
    private readonly NoiseSettings.SimpleNoiseSettings _noiseSettings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings noiseSettings)
    {
        _noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;

        var amplitude = 1f;
        var frequency = _noiseSettings.baseRoughness;

        for (var i = 0; i < _noiseSettings.numLayers; i++)
        {
            var v = _noise.Evaluate(point * frequency + _noiseSettings.centre);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= _noiseSettings.roughness;
            amplitude *= _noiseSettings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _noiseSettings.minValue);
        return noiseValue * _noiseSettings.strength;
    }
}