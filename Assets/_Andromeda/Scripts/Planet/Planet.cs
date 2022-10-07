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
    public PlanetColorSettings ColorSettings { get; private set; }

    public FaceRenderMask FaceRenderMaskValue => faceRenderMask;
    [SerializeField] private PlanetMeshGenerator planetMeshGenerator;
    [SerializeField] private ColorGenerator colorGenerator;
    public INoiseFilter[] NoiseFilters { get; private set; }

    public MinMax elevationMinMax;

    private void OnValidate()
    {
        if (planetGenerationSettingsAsset == null) return;
        planetGenerationSettingsAsset.Validated -= OnValidateManual;
        planetGenerationSettingsAsset.Validated += OnValidateManual;
        planetGenerationSettingsAsset.colorSettings.Validated -= OnValidateManual;
        planetGenerationSettingsAsset.colorSettings.Validated += OnValidateManual;
        foreach (var noiseLayer in planetGenerationSettingsAsset.noiseLayers)
        {
            noiseLayer.Validated -= OnValidateManual;
            noiseLayer.Validated += OnValidateManual;
        }

        PlanetSettings = new PlanetSettings(planetGenerationSettingsAsset);
        ColorSettings = new PlanetColorSettings(planetGenerationSettingsAsset.colorSettings);
        NoiseFilters = new INoiseFilter[PlanetSettings.noiseLayers.Count];
        for (var i = 0; i < NoiseFilters.Length; i++)
        {
            NoiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(PlanetSettings.noiseLayers[i].noiseSettings);
        }

        elevationMinMax = new MinMax();
        colorGenerator.UpdateSettings(this);
        planetMeshGenerator.CreatePlanet(this,colorGenerator);
    }

    private void OnValidateManual() => OnValidate();
}

public class PlanetSettings
{
    public readonly float radius;
    public readonly int resolution;
    public readonly List<NoiseLayerSettingsAsset> noiseLayers = new();

    public PlanetSettings(PlanetGenerationSettingsAsset planetGenerationSettingsAsset)
    {
        radius = Random.Range(planetGenerationSettingsAsset.minRadius, planetGenerationSettingsAsset.maxRadius);
        resolution = planetGenerationSettingsAsset.planetResolution;
        for (var i = 0; i < planetGenerationSettingsAsset.noiseLayers.Count; i++)
        {
            noiseLayers.Add(planetGenerationSettingsAsset.noiseLayers[i]);
        }
    }
}

public class PlanetColorSettings
{
    public readonly Gradient gradient;
    public readonly Material planetMaterial;

    public PlanetColorSettings(PlanetColorSettingsAsset asset)
    {
        planetMaterial = asset.material;
        gradient = asset.gradient;
    }
}