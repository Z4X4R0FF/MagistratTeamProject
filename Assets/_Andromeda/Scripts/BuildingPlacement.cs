using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingPlacement : MonoBehaviourSingleton<BuildingPlacement>
{
    private const float DistanceToPlace = 10f;

    public bool PlaceBuilding(BuildingAttributes attributes)
    {
        var nearestPlanet =
            WorldInfo.Instance.planetObjectsInfos.OrderBy(r => Vector3.Distance(transform.position, r.PlanetPosition))
                .First();

        switch (attributes.type)
        {
            case BuildingType.Harvest:
                var nearestOres = nearestPlanet.Generator.resourceObjects
                    .Where(r => r.Key.GetResourceType() == attributes.harvestAttributes.harvestResourceType)
                    .Where(r => Vector3.Distance(transform.position, r.Value.transform.position) <= DistanceToPlace)
                    .Where(r =>
                        WorldInfo.Instance.placedBuildings.All(b =>
                            Vector3.Distance(b.Value.transform.localPosition, r.Value.transform.localPosition) >
                            WorldInfo.MinDistanceBetweenBuildings))
                    .OrderBy(r => Vector3.Distance(transform.position, r.Value.transform.position)).ToList();
                if (nearestOres.Count != 0)
                {
                    var nearestOre = nearestOres.First();
                    if (ResourcesManager.Instance.CurrentMetal >= attributes.metalCost)
                    {
                        ResourcesManager.Instance.WithdrawResource(ResourceType.Metal, attributes.metalCost);
                        var nearestOreTransform = nearestOre.Value.transform;
                        var building = Instantiate(attributes.prefab, nearestOreTransform.position,
                            nearestOreTransform.rotation, nearestPlanet.Generator.GetBuildingsParent());
                        WorldInfo.Instance.RegisterBuilding(building.GetComponent<Building>(), nearestOre.Value);
                        return true;
                    }
                }

                break;
            case BuildingType.Attack:
            case BuildingType.Defence:
            case BuildingType.Housing:
                var nearestPlacePoints = nearestPlanet.Generator.spawnPoints
                    .Where(r => Vector3.Distance(transform.position, r.transform.position) <= DistanceToPlace)
                    .Where(r => nearestPlanet.Generator.takenPoints.All(d =>
                        d.transform.localPosition != r.transform.localPosition))
                    .Where(r =>
                        WorldInfo.Instance.placedBuildings.All(b =>
                            Vector3.Distance(b.Value.transform.localPosition, r.transform.localPosition) >
                            WorldInfo.MinDistanceBetweenBuildings))
                    .OrderBy(r => Vector3.Distance(transform.position, r.transform.position)).ToList();
                if (nearestPlacePoints.Count != 0)
                {
                    var nearestPlacePoint = nearestPlacePoints.First();
                    if (ResourcesManager.Instance.CurrentMetal >= attributes.metalCost)
                    {
                        ResourcesManager.Instance.WithdrawResource(ResourceType.Metal, attributes.metalCost);
                        var building = Instantiate(attributes.prefab, nearestPlacePoint.transform.position,
                            nearestPlacePoint.transform.rotation,
                            nearestPlanet.Generator.GetBuildingsParent());
                        WorldInfo.Instance.RegisterBuilding(building.GetComponent<Building>(), nearestPlacePoint);
                        return true;
                    }
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }
}