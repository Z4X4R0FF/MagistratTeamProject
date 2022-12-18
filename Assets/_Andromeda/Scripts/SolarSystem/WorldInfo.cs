using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WorldInfo : MonoBehaviourSingleton<WorldInfo>
{
    public readonly Dictionary<EntityTag, List<HealthComponent>> entitiesByTag =
        new()
        {
            { EntityTag.AIDamageable, new List<HealthComponent>() },
            { EntityTag.PlayerDamageable, new List<HealthComponent>() }
        };

    public List<PlanetObjectsInfo> planetObjectsInfos = new();

    public Dictionary<Building, GameObject> placedBuildings = new();

    public void RegisterEntity(HealthComponent healthComponent)
    {
        if (!entitiesByTag[healthComponent.EntityTag].Contains(healthComponent))
        {
            entitiesByTag[healthComponent.EntityTag].Add(healthComponent);
            healthComponent.onEntityDestroyed.AddListener(UnregisterEntity);
        }
    }

    private void UnregisterEntity(HealthComponent healthComponent)
    {
        if (entitiesByTag[healthComponent.EntityTag].Contains(healthComponent))
        {
            entitiesByTag[healthComponent.EntityTag].Remove(healthComponent);
        }
    }

    public void RegisterPlanet(PlanetObjectsGenerator generator)
    {
        planetObjectsInfos.Add(new PlanetObjectsInfo(generator));
    }

    public void RegisterBuilding(Building building, GameObject placePoint)
    {
        placedBuildings.Add(building, placePoint);
    }

    public void UnregisterBuilding(Building building)
    {
        placedBuildings.Remove(building);
    }

    public class PlanetObjectsInfo
    {
        public readonly PlanetObjectsGenerator Generator;

        public Vector3 PlanetPosition => Generator.transform.position;

        public PlanetObjectsInfo(PlanetObjectsGenerator generator)
        {
            Generator = generator;
        }
    }
}