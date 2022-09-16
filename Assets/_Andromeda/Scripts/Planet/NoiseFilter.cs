using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NoiseFilter
{
    private readonly Noise _noise = new();
    private readonly float _strength;
    private readonly float _roughness;
    private readonly float _baseRoughness;
    private readonly Vector3 _centre;
    private readonly float _persistance;
    private readonly int _layersCount;
    private readonly float _minValue;

    public NoiseFilter(float strength, float roughness, float baseRoughness, Vector3 centre, float persistance,
        int layersCount, float minValue)
    {
        _strength = strength;
        _roughness = roughness;
        _baseRoughness = baseRoughness;
        _centre = centre;
        _layersCount = layersCount;
        _persistance = persistance;
        _minValue = minValue;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        var frequency = _baseRoughness;
        var amplitude = 1f;

        for (var i = 0; i < _layersCount; i++)
        {
            var v = _noise.Evaluate(point * frequency + _centre);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= _roughness;
            amplitude *= _persistance;
        }

        noiseValue = Mathf.Max(0, noiseValue - _minValue);
        return noiseValue * _strength;
    }
}