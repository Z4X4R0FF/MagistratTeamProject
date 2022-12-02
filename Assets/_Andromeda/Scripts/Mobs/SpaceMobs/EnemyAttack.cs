using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public bool enableDrawRay = true;
    private List<Laser> lasers;
    private Transform myTransform;
    private Vector3 hitPosition;
    private Transform _attackTarget;

    private void Awake()
    {
        myTransform = transform;
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
        var directionToTarget = myTransform.position - _attackTarget.position;
        var angle = Vector3.Angle(myTransform.forward, directionToTarget);

        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            if (enableDrawRay) Debug.DrawLine(transform.position, _attackTarget.position, Color.green);
            return true;
        }

        if (enableDrawRay) Debug.DrawLine(transform.position, _attackTarget.position, Color.yellow);
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