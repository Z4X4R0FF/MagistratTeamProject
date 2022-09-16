using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMesh : MonoBehaviour
{
    [SerializeField, HideInInspector] private MeshFilter[] meshFilters;
    private TerrainFace[] _terrainFaces;
    private Planet _planet;

    public void CreatePlanet(Planet planet)
    {
        _planet = planet;
        Init();
        GenerateMesh();
        GenerateColors();
    }

    private void Init()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        _terrainFaces = new TerrainFace[6];

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
                meshObj.AddComponent<MeshRenderer>().sharedMaterial =
                    new Material(Shader.Find("Universal Render Pipeline/Lit"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            _terrainFaces[i] = new TerrainFace(_planet, meshFilters[i].sharedMesh, directions[i]);
        }
    }

    private void GenerateMesh()
    {
        foreach (var face in _terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    private void GenerateColors()
    {
        foreach (var meshFilter in meshFilters)
        {
            meshFilter.GetComponent<MeshRenderer>().sharedMaterial.color = _planet.color;
        }
    }
}