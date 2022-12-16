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
}