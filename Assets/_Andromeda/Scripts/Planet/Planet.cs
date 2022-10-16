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
    [SerializeField] private bool isOnValidateEnabled;
    public int tempOffsetX;
    public INoiseFilter[] NoiseFilters { get; private set; }

    public MinMax elevationMinMax;

    private void OnValidate()
    {
        if (!isOnValidateEnabled) return;
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

        SetPlanetPosition(tempOffsetX);

        PlanetSettings = new PlanetSettings(planetGenerationSettingsAsset);
        ColorSettings = new PlanetColorSettings(planetGenerationSettingsAsset.colorSettings);
        NoiseFilters = new INoiseFilter[PlanetSettings.noiseLayers.Count];
        for (var i = 0; i < NoiseFilters.Length; i++)
        {
            if (PlanetSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.useRandomCentre)
            {
                PlanetSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.centre =
                    Random.onUnitSphere * Random.Range(1, 1000);
                Debug.Log(PlanetSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.centre);
            }

            NoiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(PlanetSettings.noiseLayers[i].noiseSettings);
        }

        elevationMinMax = new MinMax();
        colorGenerator.UpdateSettings(this);
        planetMeshGenerator.CreatePlanet(this, colorGenerator);
    }

    private void OnValidateManual() => OnValidate();

    private void SetPlanetPosition(int offsetX)
    {
        transform.localPosition = new Vector3(offsetX, 0, 0);
    }

    public void EditorGeneratePlanet(int offsetX)
    {
        tempOffsetX = offsetX;
        OnValidateManual();
    }

    public void GeneratePlanet(int offsetX, PlanetGenerationSettingsAsset planetSettings)
    {
        if (planetGenerationSettingsAsset == null) return;

        PlanetSettings = new PlanetSettings(planetSettings);
        ColorSettings = new PlanetColorSettings(planetSettings.colorSettings);
        NoiseFilters = new INoiseFilter[PlanetSettings.noiseLayers.Count];
        for (var i = 0; i < NoiseFilters.Length; i++)
        {
            if (PlanetSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.useRandomCentre)
            {
                PlanetSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.centre =
                    Random.onUnitSphere * Random.Range(1, 1000);
                Debug.Log(PlanetSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings.centre);
            }

            NoiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(PlanetSettings.noiseLayers[i].noiseSettings);
        }

        elevationMinMax = new MinMax();
        colorGenerator.UpdateSettings(this);
        planetMeshGenerator.CreatePlanet(this, colorGenerator);
        SetPlanetPosition(offsetX);
        Debug.Log(
            $"Planet: resolution {PlanetSettings.resolution}; radius {PlanetSettings.radius}; position {transform.localPosition}");
    }
}

public class PlanetSettings
{
    public readonly float radius;
    public readonly int resolution;
    public readonly List<NoiseLayerSettingsAsset> noiseLayers = new();

    public PlanetSettings(PlanetGenerationSettingsAsset planetGenerationSettingsAsset)
    {
        radius = Random.Range(planetGenerationSettingsAsset.minRadius, planetGenerationSettingsAsset.maxRadius)
            .Round(10);
        //resolution = Mathf.RoundToInt(radius * planetGenerationSettingsAsset.planetResolutionScaleFactor).Round(10);
        resolution = Mathf.RoundToInt(radius * ((planetGenerationSettingsAsset.maxRadius / radius) *
                                                planetGenerationSettingsAsset.planetResolutionScaleFactor)).Round(10);
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
        planetMaterial = new Material(asset.material.shader);
        gradient = asset.gradient;
    }
}