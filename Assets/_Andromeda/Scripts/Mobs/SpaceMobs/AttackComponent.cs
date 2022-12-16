using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public bool enableDrawRay = true;
    private List<Laser> lasers;
    [SerializeField] private Transform forwardTransform;
    private Vector3 hitPosition;
    private Transform _attackTarget;
    [SerializeField] private int sightAngle = 90;

    private void Awake()
    {
        if (forwardTransform == null)
            forwardTransform = transform;
    }

    public void Init(WeaponAttributes weaponAttributes, EntityTag enemyTag)
    {
        lasers = GetComponentsInChildren<Laser>().ToList();
        foreach (var laser in lasers)
        {
            laser.Init(weaponAttributes, enemyTag);
        }
    }

    public void UpdateTarget(Transform newTarget) => _attackTarget = newTarget;

    // Update is called once per frame
    private void Update()
    {
        if (_attackTarget != null)
        {
            if (InFront() && HaveLineOfSightRayCast())
            {
                Fire();
            }
        }
    }

    private bool InFront()
    {
        var directionToTarget = forwardTransform.position - _attackTarget.position;
        var angle = Vector3.Angle(forwardTransform.forward, directionToTarget);
        //Debug.Log(angle);
        if (Mathf.Abs(angle) > 180 - sightAngle && Mathf.Abs(angle) < 180 + sightAngle)
        {
            if (enableDrawRay) Debug.DrawLine(forwardTransform.position, _attackTarget.position, Color.green);
            return true;
        }

        if (enableDrawRay) Debug.DrawLine(forwardTransform.position, _attackTarget.position, Color.yellow);
        return false;
    }

    private bool HaveLineOfSightRayCast()
    {
        foreach (var laser in lasers)
        {
            var dir = _attackTarget.position - laser.transform.position;
            if (enableDrawRay) Debug.DrawRay(laser.transform.position, dir, Color.magenta);
            if (Physics.Raycast(laser.transform.position, dir, out var hit, laser.Distance))
            {
                if (hit.transform == _attackTarget)
                {
                    if (enableDrawRay) Debug.DrawRay(laser.transform.position, dir, Color.red);
                    hitPosition = hit.transform.position;
                    return true;
                }
            }
        }

        return false;
    }

    private void Fire()
    {
        foreach (var laser in lasers)
        {
            Debug.Log($"{gameObject.name}: Fire");
            laser.FireLaser(hitPosition, _attackTarget);
        }
    }
}