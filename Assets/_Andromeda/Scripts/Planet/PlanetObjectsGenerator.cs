using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetObjectsGenerator : MonoBehaviour
{
    private readonly List<GameObject> _resourceObjects = new();
    private readonly List<GameObject> _spawnerObjects = new();
    private readonly List<GameObject> _propObjects = new();
    private List<Vector3> _pointsForSpawn = new();
    private const float MinDistanceBetweenEqualObjects = 25f;
    private Planet _planet;

    [SerializeField] private Transform resourceParent;
    [SerializeField] private Transform propParent;
    [SerializeField] private Transform spawnerParent;
    [SerializeField] private List<GameResourceAsset> resourceAssets;

    private enum PlanetObjectsTypes
    {
        Resource,
        Environment,
        Spawner
    }

    public void Init(List<Vector3> pointsForSpawn, Planet planet)
    {
        _pointsForSpawn = pointsForSpawn;
        _planet = planet;
    }

    public void GenerateResources()
    {
        var mainResourcePrefab =
            resourceAssets.FirstOrDefault(r => r.type == _planet.ResourceSettings.mainResource)?.prefab;
        var secondResourcePrefab =
            resourceAssets.FirstOrDefault(r => r.type == _planet.ResourceSettings.secondaryResource)?.prefab;
        var thirdResourcePrefab =
            resourceAssets.FirstOrDefault(r => r.type == _planet.ResourceSettings.thirdResource)?.prefab;
        for (var i = 0; i < _planet.ResourceSettings.mainResourceSpawnCount; i++)
        {
            var go = Instantiate(mainResourcePrefab, resourceParent);
            PositionObjectOnPlanet(PlanetObjectsTypes.Resource, go);
        }

        for (var i = 0; i < _planet.ResourceSettings.secondaryResourceSpawnCount; i++)
        {
            var go = Instantiate(secondResourcePrefab, resourceParent);
            PositionObjectOnPlanet(PlanetObjectsTypes.Resource, go);
        }

        for (var i = 0; i < _planet.ResourceSettings.thirdResourceSpawnCount; i++)
        {
            var go = Instantiate(thirdResourcePrefab, resourceParent);
            PositionObjectOnPlanet(PlanetObjectsTypes.Resource, go);
        }
    }

    public void GenerateSpawners()
    {
        for (var i = 0; i < _planet.RaceSettings.raceSpawnerCount; i++)
        {
            var go = Instantiate(_planet.RaceSettings.spawner, spawnerParent);
            PositionObjectOnPlanet(PlanetObjectsTypes.Spawner, go);
        }
    }

    public void GeneratePropObjects()
    {
        for (var i = 0; i < _planet.PropSettings.propSpawnCount; i++)
        {
            var go = Instantiate(_planet.PropSettings.props[Random.Range(0, _planet.PropSettings.props.Count)],
                propParent);
            PositionObjectOnPlanet(PlanetObjectsTypes.Environment, go);
        }
    }

    private void PositionObjectOnPlanet(PlanetObjectsTypes type, GameObject spawnedObject)
    {
        var iterator = 0;
        while (true)
        {
            var spawnPoint = _pointsForSpawn[Random.Range(0, _pointsForSpawn.Count)];

            switch (type)
            {
                case PlanetObjectsTypes.Resource:
                    if (iterator > 1000 || _resourceObjects.All(go =>
                            Vector3.Distance(go.transform.localPosition, spawnPoint) > MinDistanceBetweenEqualObjects))
                    {
                        spawnedObject.transform.localPosition = spawnPoint;
                        spawnedObject.transform.up = -(Vector3.zero - spawnedObject.transform.position).normalized;
                        _resourceObjects.Add(spawnedObject);
                        return;
                    }

                    break;
                case PlanetObjectsTypes.Environment:
                    if (iterator > 1000 || _propObjects.All(go =>
                            Vector3.Distance(go.transform.localPosition, spawnPoint) > MinDistanceBetweenEqualObjects))
                    {
                        spawnedObject.transform.localPosition = spawnPoint;
                        spawnedObject.transform.up = -(Vector3.zero - spawnedObject.transform.position).normalized;
                        _propObjects.Add(spawnedObject);
                        return;
                    }

                    break;
                case PlanetObjectsTypes.Spawner:
                    if (iterator > 1000 || _spawnerObjects.All(go =>
                            Vector3.Distance(go.transform.localPosition, spawnPoint) > MinDistanceBetweenEqualObjects))
                    {
                        spawnedObject.transform.localPosition = spawnPoint;
                        spawnedObject.transform.up = -(Vector3.zero - spawnedObject.transform.position).normalized;
                        _spawnerObjects.Add(spawnedObject);
                        return;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            iterator++;
        }
    }
}