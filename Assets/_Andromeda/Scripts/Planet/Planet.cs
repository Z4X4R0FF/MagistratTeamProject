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

    public PlanetSettings Settings { get; private set; }
    public PlanetColorSettings ColorSettings { get; private set; }
    public PlanetResourceSettings ResourceSettings { get; private set; }
    public PlanetPropSettings PropSettings { get; private set; }
    public PlanetRaceSettings RaceSettings { get; private set; }

    public FaceRenderMask FaceRenderMaskValue => faceRenderMask;
    [SerializeField] private PlanetMeshGenerator planetMeshGenerator;
    [SerializeField] private PlanetObjectsGenerator planetObjectsGenerator;
    [SerializeField] private ColorGenerator colorGenerator;
    [SerializeField] private bool isOnValidateEnabled;
    public int tempOffsetX;
    public INoiseFilter[] NoiseFilters { get; private set; }

    public MinMax elevationMinMax;

    public List<Vector3> FreePointsToLand => planetObjectsGenerator.PointsForSpawn;

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

        GeneratePlanet(tempOffsetX,planetGenerationSettingsAsset);
    }

    private void OnValidateManual() => OnValidate();

    private void SetPlanetPosition(int offsetX)
    {
        transform.localPosition = new Vector3(offsetX, 0, 0);
    }

    public void GeneratePlanet(int offsetX, PlanetGenerationSettingsAsset planetSettings)
    {
        Settings = new PlanetSettings(planetSettings);
        ColorSettings = new PlanetColorSettings(planetSettings.colorSettings);
        ResourceSettings =
            new PlanetResourceSettings(planetSettings.resourceSettings, Settings.radius);
        PropSettings = new PlanetPropSettings(planetSettings.propSettings, Settings.radius);
        //RaceSettings = new PlanetRaceSettings(planetSettings.mobAsset, Settings.radius);

        NoiseFilters = new INoiseFilter[Settings.noiseLayers.Count];
        for (var i = 0; i < NoiseFilters.Length; i++)
        {
            if (Settings.noiseLayers[i].noiseSettings.simpleNoiseSettings.useRandomCentre)
            {
                Settings.noiseLayers[i].noiseSettings.simpleNoiseSettings.centre =
                    Random.onUnitSphere * Random.Range(1, 1000);
            }

            NoiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(Settings.noiseLayers[i].noiseSettings);
        }

        elevationMinMax = new MinMax();
        colorGenerator.UpdateSettings(this);
        planetMeshGenerator.CreatePlanet(this, colorGenerator);
        SetPlanetPosition(offsetX);
        Debug.Log(
            $"Planet: resolution {Settings.resolution}; radius {Settings.radius}; position {transform.localPosition}");
        Debug.Log(
            $"PlanetResources: Main resource {ResourceSettings.mainResource.ToString()} count: {ResourceSettings.mainResourceSpawnCount}/n" +
            $"Second resource {ResourceSettings.secondaryResource.ToString()} count: {ResourceSettings.secondaryResourceSpawnCount}/n" +
            $"Third resource {ResourceSettings.thirdResource.ToString()} count: {ResourceSettings.thirdResourceSpawnCount}");
    }

    #region PlanetProperties

    public class PlanetSettings
    {
        public readonly int radius;
        public readonly int resolution;
        public readonly float spawnHeightPercentage;
        public readonly List<NoiseLayerSettingsAsset> noiseLayers = new();

        public PlanetSettings(PlanetGenerationSettingsAsset planetGenerationSettingsAsset)
        {
            radius = Random.Range(planetGenerationSettingsAsset.minRadius, planetGenerationSettingsAsset.maxRadius)
                .Round(10);
            //resolution = Mathf.RoundToInt(radius * planetGenerationSettingsAsset.planetResolutionScaleFactor).Round(10);
            resolution = Mathf.RoundToInt(radius * (planetGenerationSettingsAsset.maxRadius / (float)radius *
                                                    planetGenerationSettingsAsset.planetResolutionScaleFactor))
                .Round(10);
            spawnHeightPercentage = planetGenerationSettingsAsset.spawnHeightPercentage;
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

    public class PlanetResourceSettings
    {
        public readonly ResourceType mainResource;
        public readonly ResourceType secondaryResource;
        public readonly ResourceType thirdResource;
        public readonly int mainResourceSpawnCount;
        public readonly int secondaryResourceSpawnCount;
        public readonly int thirdResourceSpawnCount;


        public PlanetResourceSettings(PlanetResourceSettingsAsset settingsAsset, int planetRadius)
        {
            mainResource = settingsAsset.mainResource;
            secondaryResource = settingsAsset.secondaryResource;
            thirdResource = settingsAsset.thirdResource;
            var totalResourceSpawnCount =
                Random.Range(settingsAsset.minResourceSpawnCount, settingsAsset.maxResourceSpawnCount) *
                (planetRadius / 10);
            mainResourceSpawnCount = (int)(totalResourceSpawnCount * settingsAsset.mainResourcePercentage);
            secondaryResourceSpawnCount = (int)((totalResourceSpawnCount - mainResourceSpawnCount) *
                                                settingsAsset.secondaryResourcePercentage);
            thirdResourceSpawnCount = totalResourceSpawnCount - mainResourceSpawnCount - secondaryResourceSpawnCount;
        }
    }

    public class PlanetPropSettings
    {
        public readonly int propSpawnCount;
        public readonly List<GameObject> props;

        public PlanetPropSettings(PlanetPropsSettingsAsset settingsAsset, int planetRadius)
        {
            props = settingsAsset.planetProps;
            propSpawnCount =
                Random.Range(settingsAsset.minPropSpawnCount, settingsAsset.maxPropSpawnCount) *
                (planetRadius / 10);
        }
    }

    public class PlanetRaceSettings
    {
        public readonly GameObject spawner;
        public readonly int raceSpawnerCount;

        public PlanetRaceSettings(MobRaceAsset asset, int planetRadius)
        {
            spawner = asset.SpawnerPrefab;
            raceSpawnerCount = Random.Range(asset.minSpawnerCount, asset.maxSpawnerCount) *
                               (planetRadius / 10);
        }
    }

    #endregion
}