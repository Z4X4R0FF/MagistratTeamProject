using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SpaceEnemyAi : MonoBehaviour
{
    [SerializeField] private SpaceEnemyMovement movement;

    public HealthComponent CurrentTarget { get; private set; }
    public Vector3 PositionTarget;
    public bool isReachedPosition;
    private bool hasTarget;
    private const float MinDistanceToTarget = 50f;
    private EntityTag _enemyTag;

    [HideInInspector] public UnityEvent<Transform> onTargetUpdated = new();

    public void Init(EntityTag enemyTag)
    {
        _enemyTag = enemyTag;
        InvokeRepeating(nameof(FindNewTarget), 10, 10);
    }

    private void Update()
    {
        FindTarget();
        if (CurrentTarget == null) hasTarget = false;
        if (hasTarget)
        {
            //Debug.Log("Distance - " + Vector3.Distance(transform.position, PositionTarget));
            if (Vector3.Distance(transform.position, PositionTarget) <= 60f)
                isReachedPosition = true;

            if (!isReachedPosition)
            {
                if (CurrentTarget.GetEntityType() == EntityType.Building)
                {
                    PositionTarget = CurrentTarget.transform.position +
                                     CurrentTarget.transform.up * MinDistanceToTarget * 10;
                }
            }
            else
            {
                PositionTarget = CurrentTarget.transform.position;
            }

            movement.SetState(Vector3.Distance(transform.position, PositionTarget) <= MinDistanceToTarget
                ? SpaceEnemyMovement.MovementState.Seeking
                : SpaceEnemyMovement.MovementState.Moving);
        }
        else
        {
            movement.SetState(SpaceEnemyMovement.MovementState.Staying);
            FindTarget();
        }
    }

    private void SetTarget(HealthComponent target)
    {
        if (target.GetEntityType() == EntityType.Building)
        {
            PositionTarget = target.transform.position + target.transform.up * MinDistanceToTarget;
            isReachedPosition = false;
        }
        else
        {
            isReachedPosition = true;
            PositionTarget = target.transform.position;
        }

        CurrentTarget = target;
        hasTarget = true;
        onTargetUpdated.Invoke(CurrentTarget.Hull);
    }

    private void FindTarget()
    {
        if (CurrentTarget == null)
        {
            if (WorldInfo.Instance.entitiesByTag[_enemyTag].Count != 0)
            {
                var target =
                    WorldInfo.Instance.entitiesByTag[_enemyTag]
                        .OrderBy(go => Vector3.Distance(transform.position, go.transform.position))
                        .First();
                SetTarget(target);
            }
        }
    }

    private void FindNewTarget()
    {
        if (WorldInfo.Instance.entitiesByTag[_enemyTag].Count != 0)
        {
            var target =
                WorldInfo.Instance.entitiesByTag[_enemyTag]
                    .OrderBy(go => Vector3.Distance(transform.position, go.transform.position))
                    .First();
            SetTarget(target);
        }
    }
}