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

    private void Update()
    {
        FindTarget();
        if (CurrentTarget == null) hasTarget = false;
        if (hasTarget)
        {
            movement.SetState(Vector3.Distance(transform.position, CurrentTarget.position) <= MinDistanceToTarget
                ? SpaceEnemyMovement.MovementState.Seeking
                : SpaceEnemyMovement.MovementState.Moving);
        }
        else
        {
            movement.SetState(SpaceEnemyMovement.MovementState.Staying);
            FindTarget();
        }
    }

    private void SetTarget(Transform target)
    {
        CurrentTarget = target;
        hasTarget = true;
        onTargetUpdated.Invoke(target);
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
                SetTarget(target.Hull);
            }
        }
    }
}