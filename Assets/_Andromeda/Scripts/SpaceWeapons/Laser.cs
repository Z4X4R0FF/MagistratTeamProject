using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField] private float laserOffTime = .25f;
    [SerializeField] private float maxDistance = 300f;
    [SerializeField] private float fireDelay = 0.5f;
    private LineRenderer lr;
    private bool canFire;

    public float Distance => maxDistance;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lr.enabled = false;
    }

    Vector3 CastRay()
    {
        var fwd = transform.TransformDirection(Vector3.forward) * maxDistance;

        if (Physics.Raycast(transform.position, fwd, out var hit))
        {
            Debug.Log($"Laser hit {hit.transform.name}");
            return hit.point;
        }
        else
        {
            return transform.position + transform.forward * maxDistance;
        }
    }

    public void FireLaser(Vector3 targetPosition)
    {
        if (canFire)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, CastRay());
            lr.enabled = true;
            canFire = false;
            Invoke(nameof(TurnOffLaser), laserOffTime);
            Invoke(nameof(CanFire), fireDelay);
        }
    }

    private void TurnOffLaser()
    {
        lr.enabled = false;
    }

    private void CanFire() => canFire = true;
}