using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private List<Laser> lasers;
    [SerializeField] private Transform AttackTarget;

    private void Start()
    {
        lasers = GetComponentsInChildren<Laser>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (InFront() && HaveLineOfSight())
        {
            Fire();
        }
    }

    bool InFront()
    {
        var directionToTarget = transform.position - AttackTarget.position;
        var angle = Vector3.Angle(transform.forward, directionToTarget);

        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            //Debug.DrawLine(transform.position, AttackTarget.position, Color.green);
            return true;
        }

        //Debug.DrawLine(transform.position, AttackTarget.position, Color.yellow);
        return false;
    }

    bool HaveLineOfSight()
    {
        foreach (var laser in lasers)
        {
            var dir = AttackTarget.position - transform.position;
            // Debug.DrawRay(laser.transform.position, dir, Color.red);
            if (Physics.Raycast(laser.transform.position, dir, out var hit, laser.Distance))
            {
                if (hit.transform == AttackTarget.transform)
                {
                    Debug.DrawRay(laser.transform.position, dir, Color.red);
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
            var pos = transform.position + transform.forward * laser.Distance;
            laser.FireLaser(pos);
        }
    }
}