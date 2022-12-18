using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetObjectsGenerator : MonoBehaviour
{
    public readonly Dictionary<ResourceVein,GameObject> resourceObjects = new();
    public readonly List<GameObject> spawnerObjects = new();
    public readonly List<GameObject> propObjects = new();
    public readonly List<GameObject> takenPoints = new();
    public readonly List<GameObject> spawnPoints = new();
    public List<Vector3> pointsForSpawn = new();
    private const float MinDistanceBetweenEqualObjects = 25f;
    private Planet _planet;

    [SerializeField] private Transform resourceParent;
    [SerializeField] private Transform propParent;
    [SerializeField] private Transform spawnerParent;
    [SerializeField] private Transform buildingsParent;
    [SerializeField] private Transform pointsParent;
    [SerializeField] private List<GameResourceAsset> resourceAssets;
    [SerializeField] private GameObject placePoint;

    public Transform GetBuildingsParent() => buildingsParent;

    private enum PlanetObjectsTypes
    {
        Resource,
        Environment,
        Spawner
    }

    public void Init(List<Vector3> pointsForSpawn, Planet planet)
    {
        //resourceObjects.ForEach(Destroy);
        spawnerObjects.ForEach(Destroy);
        propObjects.ForEach(Destroy);
        resourceObjects.Clear();
        spawnerObjects.Clear();
        propObjects.Clear();
        this.pointsForSpawn = pointsForSpawn;
        _planet = planet;

        foreach (var point in pointsForSpawn)
        {
            var go = Instantiate(placePoint, pointsParent);
            go.transform.localPosition = point;
            go.transform.up = -(Vector3.zero - go.transform.position).normalized;
            spawnPoints.Add(go);
        }
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
            var spawnPoint = spawnPoints[Random.Range(0, pointsForSpawn.Count)];

            switch (type)
            {
                case PlanetObjectsTypes.Resource:
                    if (iterator >= 1000)
                    {
                        Debug.Log($"Can't place {type}. Iterations exceeded");
                        return;
                    }

                    if (resourceObjects.All(go =>
                            Vector3.Distance(go.Value.transform.localPosition, spawnPoint.transform.localPosition) >
                            MinDistanceBetweenEqualObjects) && !takenPoints.Contains(spawnPoint))
                    {
                        spawnedObject.transform.localPosition = spawnPoint.transform.localPosition;
                        spawnedObject.transform.localRotation =
                            Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                        spawnedObject.transform.up = -(Vector3.zero - spawnedObject.transform.position).normalized;
                        resourceObjects.Add(spawnedObject.GetComponent<ResourceVein>(),spawnPoint);
                        takenPoints.Add(spawnPoint);

                        return;
                    }

                    break;
                case PlanetObjectsTypes.Environment:
                    if (iterator >= 1000)
                    {
                        Debug.Log($"Can't place {type}. Iterations exceeded");
                        return;
                    }

                    if (propObjects.All(go =>
                            Vector3.Distance(go.transform.localPosition, spawnPoint.transform.localPosition) >
                            MinDistanceBetweenEqualObjects) && !takenPoints.Contains(spawnPoint))
                    {
                        spawnedObject.transform.localPosition = spawnPoint.transform.localPosition;
                        spawnedObject.transform.localRotation =
                            Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                        spawnedObject.transform.up = -(Vector3.zero - spawnedObject.transform.position).normalized;
                        propObjects.Add(spawnedObject);
                        takenPoints.Add(spawnPoint);
                        return;
                    }

                    break;
                case PlanetObjectsTypes.Spawner:
                    // if (iterator >= 1000)
                    // {
                    //     Debug.Log($"Can't place {type}. Iterations exceeded");
                    //     return;
                    // }
                    //
                    // if (spawnerObjects.All(go =>
                    //         Vector3.Distance(go.transform.localPosition, spawnPoint) >
                    //         MinDistanceBetweenEqualObjects) && !takenPoints.Contains(spawnPoint))
                    // {
                    //     spawnedObject.transform.localPosition = spawnPoint;
                    //     spawnedObject.transform.localRotation =
                    //         Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                    //     spawnedObject.transform.up = -(Vector3.zero - spawnedObject.transform.position).normalized;
                    //     spawnerObjects.Add(spawnedObject);
                    //     takenPoints.Add(spawnPoint);
                    //     return;
                    // }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            iterator++;
        }
    }
}