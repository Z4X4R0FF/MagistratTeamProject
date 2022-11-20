using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpaceEnemyAi : MonoBehaviour
{
    [SerializeField] private SpaceEnemyMovement movement;

    public Transform CurrentTarget { get; private set; }
    private bool hasTarget;
    private const float MinDistance = 50f;

    private void Start()
    {
        FindTarget();
    }

    private void Update()
    {
        if (hasTarget)
        {
            movement.SetState(Vector3.Distance(transform.position, CurrentTarget.position) <= MinDistance
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
    }

    private void FindTarget()
    {
        if (CurrentTarget == null)
        {
            var target =
                WorldInfo.Instance.aiDamageableEntities.OrderBy(go => Vector3.Distance(transform.position, go.transform.position))
                    .First();
            SetTarget(target.transform);
        }
    }
}