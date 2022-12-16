using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour
{
    [SerializeField] private TurretAI ai;

    [SerializeField] private float rotationalDump = .5f;

    [SerializeField] private Transform turretHead;

    private TurretMovementState _turretMovementState;

    // Update is called once per frame
    private void Update()
    {
        if (ai.CurrentTarget == null)
        {
            _turretMovementState = TurretMovementState.Staying;
        }

        switch (_turretMovementState)
        {
            case TurretMovementState.Staying:
                break;
            case TurretMovementState.Seeking:
                Turn();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Turn()
    {
        var pos = ai.CurrentTarget.position - turretHead.position;
        var rot = Quaternion.LookRotation(pos);
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, rot, rotationalDump * Time.deltaTime);
    }

    public void SetState(TurretMovementState state) => _turretMovementState = state;

    public enum TurretMovementState
    {
        Staying,
        Seeking
    }
}