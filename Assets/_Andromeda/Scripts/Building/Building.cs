using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class Building : MonoBehaviour
{
    [SerializeField] private BuildingAttributes buildingAttributes;

    private SpaceEnemyMovement _spaceEnemyMovement;
    private HealthComponent _healthComponent;
    private TurretAI _aiComponent;
    private AttackComponent _attackComponent;

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.Init(buildingAttributes.healthAttributes);
        switch (buildingAttributes.type)
        {
            case BuildingType.Harvest:
                ResourcesManager.Instance.UpdateResourceYield(buildingAttributes.harvestAttributes.harvestResourceType,
                    buildingAttributes.harvestAttributes.harvestAmount, false);
                break;
            case BuildingType.Attack:
                _attackComponent = GetComponent<AttackComponent>();
                _attackComponent.Init(buildingAttributes.weaponAttributes, buildingAttributes.enemyTag);

                _aiComponent = GetComponent<TurretAI>();
                _aiComponent.Init(buildingAttributes.enemyTag);
                _aiComponent.onTargetUpdated.AddListener(UpdateAttackTarget);
                break;
            case BuildingType.Defence:
                break;
            case BuildingType.Housing:
                ResourcesManager.Instance.UpdatePeople(buildingAttributes.housingAttributes.capacity, false);
                ResourcesManager.Instance.UpdateResourceYield(ResourceType.Meals,
                    buildingAttributes.housingAttributes.mealsPerTickCost, true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        ResourcesManager.Instance.UpdateResourceYield(ResourceType.Uranium, buildingAttributes.uraniumPerTickCost,
            true);
    }

    private void OnDestroy()
    {
        switch (buildingAttributes.type)
        {
            case BuildingType.Harvest:
                ResourcesManager.Instance.UpdateResourceYield(buildingAttributes.harvestAttributes.harvestResourceType,
                    buildingAttributes.harvestAttributes.harvestAmount, true);
                break;
            case BuildingType.Attack:
                break;
            case BuildingType.Defence:
                break;
            case BuildingType.Housing:
                ResourcesManager.Instance.UpdatePeople(buildingAttributes.housingAttributes.capacity, true);
                ResourcesManager.Instance.UpdateResourceYield(ResourceType.Meals,
                    buildingAttributes.housingAttributes.mealsPerTickCost, false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        ResourcesManager.Instance.UpdateResourceYield(ResourceType.Uranium, buildingAttributes.uraniumPerTickCost,
            false);
        WorldInfo.Instance.UnregisterBuilding(this);
    }

    private void UpdateAttackTarget(Transform newTarget)
    {
        _attackComponent.UpdateTarget(newTarget);
    }
}