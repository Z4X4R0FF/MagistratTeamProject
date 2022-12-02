using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetMeshGenerator : MonoBehaviour
{
    [SerializeField] private PlanetObjectsGenerator objectsGenerator;
    [SerializeField, HideInInspector] private MeshFilter[] meshFilters;
    private TerrainFace[] _terrainFaces;
    private Planet _planet;
    private ColorGenerator _colorGenerator;
    private readonly List<Vector3> objectSpawnPoints = new();

    public void CreatePlanet(Planet planet, ColorGenerator colorGenerator)
    {
        _planet = planet;
        _colorGenerator = colorGenerator;
        UpdateSettings();
        GenerateMesh();
        GetPossibleSpawnPoints();
        GenerateColors();
        objectsGenerator.Init(objectSpawnPoints, _planet);
        objectsGenerator.GenerateResources();
        // objectsGenerator.GenerateSpawners();
        objectsGenerator.GeneratePropObjects();
    }

    private void UpdateSettings()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        _terrainFaces = new TerrainFace[6];
        objectSpawnPoints.Clear();

        var directions = new[]
            { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        for (var i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i] == null)
            {
                var meshObj = new GameObject("mesh")
                {
                    transform =
                    {
                        parent = transform,
                        localPosition = Vector3.zero
                    }
                };
                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = _planet.ColorSettings.planetMaterial;

            _terrainFaces[i] = new TerrainFace(_planet, meshFilters[i].sharedMesh, directions[i],
                _planet.Settings.radius);
            var renderFace = _planet.FaceRenderMaskValue == Planet.FaceRenderMask.All ||
                             (int)_planet.FaceRenderMaskValue - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    private void GenerateMesh()
    {
        for (var i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                _terrainFaces[i].ConstructMesh();
            }
        }

        _colorGenerator.UpdateElevation(_planet.elevationMinMax);
        Debug.Log($"ElevationMinMax {_planet.elevationMinMax.Min}  {_planet.elevationMinMax.Max}");
    }

    private void GenerateColors()
    {
        _colorGenerator.UpdateColors();
    }

    private void GetPossibleSpawnPoints()
    {
        var maxHeight = Mathf.Lerp(_planet.elevationMinMax.Min, _planet.elevationMinMax.Max,
            _planet.Settings.spawnHeightPercentage);
        Debug.Log($"MaxSpawnHeight: {maxHeight}");
        foreach (var terrainFace in meshFilters)
        {
            foreach (var vert in terrainFace.sharedMesh.vertices)
            {
                if (Vector3.Distance(vert, Vector3.zero) < maxHeight)
                    objectSpawnPoints.Add(vert);
            }
        }

        Debug.Log(objectSpawnPoints.Count);
    }
}