using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(SpaceEnemyAi))]
[RequireComponent(typeof(SpaceEnemyMovement))]
[RequireComponent(typeof(HealthComponent))]
public class SpaceEnemy : MonoBehaviour
{
    [SerializeField] private ShipAttributes shipAttributes;

    private SpaceEnemyMovement _spaceEnemyMovement;
    private HealthComponent _healthComponent;
    private SpaceEnemyAi _aiComponent;
    private EnemyAttack _attackComponent;

    private void Awake()
    {
        _spaceEnemyMovement = GetComponent<SpaceEnemyMovement>();
        _spaceEnemyMovement.Init(shipAttributes.aiMovementAttributes);

        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.Init(shipAttributes.healthAttributes);

        _attackComponent = GetComponent<EnemyAttack>();
        _attackComponent.Init(shipAttributes.weaponAttributes, shipAttributes.enemyTag);

        _aiComponent = GetComponent<SpaceEnemyAi>();
        _aiComponent.Init(shipAttributes.enemyTag);
        _aiComponent.onTargetUpdated.AddListener(UpdateAttackTarget);
    }

    private void UpdateAttackTarget(Transform newTarget)
    {
        _attackComponent.UpdateTarget(newTarget);
    }
}