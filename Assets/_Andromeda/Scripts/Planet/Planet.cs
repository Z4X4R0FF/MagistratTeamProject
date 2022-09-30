using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    #region Planet Parameters

    public enum FaceRenderMask
    {
        All,
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    }

    [SerializeField] private FaceRenderMask faceRenderMask;

    [SerializeField] private PlanetGenerationSettingsAsset planetGenerationSettingsAsset;

    #endregion

    public PlanetSettings PlanetSettings { get; private set; }

    public FaceRenderMask FaceRenderMaskValue => faceRenderMask;
    [SerializeField] private PlanetMesh planetMesh;
    public INoiseFilter[] NoiseFilters { get; private set; }

    private void OnValidate()
    {
        if (planetGenerationSettingsAsset == null) return;
        planetGenerationSettingsAsset.Validated -= OnValidateManual;
        planetGenerationSettingsAsset.Validated += OnValidateManual;
        foreach (var noiseLayer in planetGenerationSettingsAsset.noiseLayers)
        {
            noiseLayer.Validated -= OnValidateManual;
            noiseLayer.Validated += OnValidateManual;
        }

        PlanetSettings = new PlanetSettings(planetGenerationSettingsAsset);
        NoiseFilters = new INoiseFilter[PlanetSettings.noiseLayers.Count];
        for (var i = 0; i < NoiseFilters.Length; i++)
        {
            NoiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(PlanetSettings.noiseLayers[i].noiseSettings);
        }

        planetMesh.CreatePlanet(this);
    }

    private void OnValidateManual() => OnValidate();
}

public class PlanetSettings
{
    public readonly float radius;
    public readonly int resolution;
    public Color color;
    public readonly List<NoiseLayerSettingsAsset> noiseLayers = new();

    public PlanetSettings(PlanetGenerationSettingsAsset planetGenerationSettingsAsset)
    {
        radius = Random.Range(planetGenerationSettingsAsset.minRadius, planetGenerationSettingsAsset.maxRadius);
        color = planetGenerationSettingsAsset.color;
        resolution = planetGenerationSettingsAsset.planetResolution;
        for (var i = 0; i < planetGenerationSettingsAsset.noiseLayers.Count; i++)
        {
            noiseLayers.Add(planetGenerationSettingsAsset.noiseLayers[i]);
        }
    }
}