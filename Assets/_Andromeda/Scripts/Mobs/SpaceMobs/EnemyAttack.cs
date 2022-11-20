using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private List<Laser> lasers;
    private Transform myTransform;
    private Vector3 hitPosition;
    [SerializeField] private Transform AttackTarget;

    private void Awake()
    {
        myTransform = transform;
    }

    private void Start()
    {
        lasers = GetComponentsInChildren<Laser>().ToList();
    }

    // Update is called once per frame
    private void Update()
    {
        if (InFront() && HaveLineOfSightRayCast())
        {
            Fire();
        }
    }

    private bool InFront()
    {
        var directionToTarget = myTransform.position - AttackTarget.position;
        var angle = Vector3.Angle(myTransform.forward, directionToTarget);

        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            Debug.DrawLine(transform.position, AttackTarget.position, Color.green);
            return true;
        }

        Debug.DrawLine(transform.position, AttackTarget.position, Color.yellow);
        return false;
    }

    private bool HaveLineOfSightRayCast()
    {
        foreach (var laser in lasers)
        {
            var dir = AttackTarget.position - myTransform.position;
            Debug.DrawRay(laser.transform.position, dir, Color.magenta);
            if (Physics.Raycast(laser.transform.position, dir, out var hit, laser.Distance))
            {
                if (hit.transform == AttackTarget.transform)
                {
                    Debug.DrawRay(laser.transform.position, dir, Color.red);
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
            Debug.Log("Fire");
            laser.FireLaser(hitPosition, AttackTarget);
        }
    }
}