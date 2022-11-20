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
    private Transform myTransform;

    public float Distance => maxDistance;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        myTransform = transform;
    }

    private void Start()
    {
        lr.enabled = false;
        CanFire();
    }

    private Vector3 CastRay()
    {
        var fwd = myTransform.TransformDirection(Vector3.forward) * maxDistance;

        if (Physics.Raycast(myTransform.position, fwd, out var hit))
        {
            Debug.Log($"Laser hit {hit.transform.name}");
            if (hit.transform.CompareTag("AIAttackable"))
            {
                SpawnLaserExplosion(hit.point, hit.transform);
            }

            return hit.point;
        }
        else
        {
            return myTransform.position + myTransform.forward * maxDistance;
        }
    }

    private void SpawnLaserExplosion(Vector3 hitPosition, Transform target)
    {
        target.GetComponent<Building>()?.OnBuildingHit(hitPosition);
    }

    public void FireLaser()
    {
        FireLaser(CastRay());
    }

    public void FireLaser(Vector3 targetPosition, Transform target = null)
    {
        if (canFire)
        {
            if (target != null)
                SpawnLaserExplosion(targetPosition, target);
            lr.SetPosition(0, myTransform.position);
            lr.SetPosition(1, targetPosition);
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