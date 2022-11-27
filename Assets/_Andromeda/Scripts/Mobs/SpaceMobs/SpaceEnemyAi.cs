using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SpaceEnemyAi : MonoBehaviour
{
    [SerializeField] private SpaceEnemyMovement movement;

    public Transform CurrentTarget { get; private set; }
    private bool hasTarget;
    private const float MinDistanceToTarget = 50f;
    private EntityTag _enemyTag;

    [HideInInspector] public UnityEvent<Transform> onTargetUpdated = new();

    public void Init(EntityTag enemyTag)
    {
        _enemyTag = enemyTag;
    }

    private void Start()
    {
        FindTarget();
    }

    private void Update()
    {
        if (hasTarget)
        {
            movement.SetState(Vector3.Distance(transform.position, CurrentTarget.position) <= MinDistanceToTarget
                ? SpaceEnemyMovement.MovementState.Staying
                : SpaceEnemyMovement.MovementState.Moving);
        }
        else
        {
            FindTarget();
        }
    }

    private void SetTarget(Transform target)
    {
        CurrentTarget = target;
        hasTarget = true;
        movement.SetState(SpaceEnemyMovement.MovementState.Moving);
        onTargetUpdated.Invoke(target);
    }

    private void FindTarget()
    {
        if (CurrentTarget == null)
        {
            var target =
                WorldInfo.Instance.entitiesByTag[_enemyTag]
                    .OrderBy(go => Vector3.Distance(transform.position, go.transform.position))
                    .First();
            SetTarget(target.transform);
        }
    }
}