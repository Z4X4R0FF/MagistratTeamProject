using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceEnemyMovement : MonoBehaviour
{
    [SerializeField] private SpaceEnemyAi ai;

    [SerializeField] private float rotationalDump = .5f;
    private float movementSpeed = 10f;
    private float evasionSpeed = 25f;

    [SerializeField] private float detectionDistance = 50f;
    [SerializeField] private float rayCastOffset = 2.5f;
    [SerializeField] private int raysCount = 8;

    private MovementState _movementState;
    private Transform myTransform;
    private float _horde;

    private void Awake()
    {
        myTransform = transform;
        _horde = rayCastOffset * Mathf.Sin(180f / raysCount * Mathf.Deg2Rad);
    }

    public void Init(MovementAttributes movementAttributes)
    {
        movementSpeed = movementAttributes.movementSpeed;
        evasionSpeed = movementAttributes.rotationSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (ai.CurrentTarget == null)
        {
            _movementState = MovementState.Staying;
        }

        switch (_movementState)
        {
            case MovementState.Moving:
                Pathfinding();
                Move();
                break;
            case MovementState.Staying:
                break;
            case MovementState.Seeking:
                Turn();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Turn()
    {
        var pos = ai.CurrentTarget.position - myTransform.position;
        var rot = Quaternion.LookRotation(pos);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, rot, rotationalDump * Time.deltaTime);
    }

    private void Move()
    {
        myTransform.position += myTransform.forward * movementSpeed * Time.deltaTime;
    }

    private void Pathfinding()
    {
        var raycastOffset = Vector3.zero;
        var trForward = myTransform.forward;

        var quaternion = Quaternion.AngleAxis(360f / raysCount, trForward);
        var oppositeQuat = Quaternion.AngleAxis(360f / 2, trForward);
        var vec3 = myTransform.up * rayCastOffset;
        for (var i = 0; i < raysCount / 2; i++)
        {
            var pos = myTransform.position + vec3;
            var oppositePos = myTransform.position + (oppositeQuat * vec3);
            vec3 = quaternion * vec3;

            //Debug.DrawRay(pos, trForward * detectionDistance, Color.cyan);
            //Debug.DrawRay(oppositePos, trForward * detectionDistance, Color.cyan);
            if (Physics.SphereCast(pos, _horde, myTransform.forward, out var hit1, detectionDistance))
            {
                Debug.Log($"{name}: Evading {hit1.transform.name}");
                raycastOffset += myTransform.InverseTransformPoint(oppositePos);
            }
            else if (Physics.SphereCast(oppositePos, _horde, myTransform.forward, out var hit2,
                         detectionDistance))
            {
                Debug.Log($"{name}: Evading {hit2.transform.name}");
                raycastOffset += myTransform.InverseTransformPoint(pos);
            }
        }

        if (raycastOffset != Vector3.zero)
        {
            myTransform.Rotate(raycastOffset * (evasionSpeed * Time.deltaTime));
        }
        else Turn();
    }

    public void SetState(MovementState state) => _movementState = state;

    public enum MovementState
    {
        Moving,
        Staying,
        Seeking
    }
}