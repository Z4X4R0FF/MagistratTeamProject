using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainFace
{
    private readonly Planet _planet;
    private readonly Mesh _mesh;
    private readonly int _resolution;
    private readonly Vector3 _localUp;
    private readonly Vector3 _axisA;
    private readonly Vector3 _axisB;

    public TerrainFace(Planet planet, Mesh mesh, Vector3 localUp)
    {
        _planet = planet;
        _mesh = mesh;
        _resolution = planet.PlanetSettings.resolution;
        _localUp = localUp;

        _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        _axisB = Vector3.Cross(localUp, _axisA);
    }

    public void ConstructMesh()
    {
        var vertices = new Vector3[_resolution * _resolution];
        var triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];
        var triIndex = 0;
        for (var y = 0; y < _resolution; y++)
        {
            for (var x = 0; x < _resolution; x++)
            {
                var i = x + y * _resolution;
                var percent = new Vector2(x, y) / (_resolution - 1);
                var pointOnUnitCube = _localUp + (percent.x - .5f) * 2 * _axisA + (percent.y - .5f) * 2 * _axisB;
                var pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = CalculatePointOnPlanet(pointOnUnitSphere);

                if (x != _resolution - 1 && y != _resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + _resolution + 1;
                    triangles[triIndex + 2] = i + _resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + _resolution + 1;
                    triIndex += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }

    private Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        var elevation = 0f;
        var firstLayerValue = 0f;

        if (_planet.NoiseFilters.Length > 0)
        {
            firstLayerValue = _planet.NoiseFilters[0].Evaluate(pointOnUnitSphere);
            if (_planet.PlanetSettings.noiseLayers[0].isEnabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (var i = 0; i < _planet.NoiseFilters.Length; i++)
        {
            if (_planet.PlanetSettings.noiseLayers[i].isEnabled)
            {
                var mask = (_planet.PlanetSettings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += _planet.NoiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        elevation = _planet.PlanetSettings.radius * (1 + elevation);
        _planet.elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;
    }
}