using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackComponent))]
[RequireComponent(typeof(HealthComponent))]
public class PlayerShip : MonoBehaviour
{
    [SerializeField] private ShipAttributes shipAttributes;

    private HealthComponent _healthComponent;
    private AttackComponent _attackComponent;

    private void Awake()
    {

        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.Init(shipAttributes.healthAttributes);

        _attackComponent = GetComponent<AttackComponent>();
        _attackComponent.Init(shipAttributes.weaponAttributes, shipAttributes.enemyTag);

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _attackComponent.PlayerFire();
        }
    }
}