using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    #region Planet Parameters

    [Header("Basic Params")] [Range(10, 200)] [SerializeField]
    public float radius = 1;

    [SerializeField] public Color color;
    [Range(2, 256)] [SerializeField] public int planetResolution = 10;

    [Header("NoiseSettings")] [SerializeField] [Range(1, 10)]
    private int noiseLayersCount = 1;

    [SerializeField] private float strength = 1;
    [SerializeField] private float baseRoughness = 1;
    [SerializeField] private float roughness = 2;
    [SerializeField] private float persistance = .5f;
    [SerializeField] private Vector3 centre;
    [SerializeField] private float minValue;

    #endregion

    [SerializeField] private PlanetMesh planetMesh;
    [HideInInspector] public NoiseFilter noiseFilter;

    private void OnValidate()
    {
        noiseFilter = new NoiseFilter(strength, roughness, baseRoughness, centre, persistance, noiseLayersCount,minValue);
        planetMesh.CreatePlanet(this);
    }
}