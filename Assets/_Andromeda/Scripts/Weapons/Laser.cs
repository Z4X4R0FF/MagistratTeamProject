using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField] private float laserOffTime = .25f;
    [SerializeField] private float maxDistance = 300f;
    private LineRenderer lr;
    private bool canFire;
    private Transform myTransform;
    private WeaponAttributes _weaponAttributes;
    private EntityTag _enemyTag;

    public float Distance => maxDistance;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        myTransform = transform;
    }

    public void Init(WeaponAttributes weaponAttributes, EntityTag enemyTag)
    {
        if (lr == null) lr = GetComponent<LineRenderer>();
        _weaponAttributes = weaponAttributes;
        lr.startColor = _weaponAttributes.weaponColor;
        lr.endColor = _weaponAttributes.weaponColor;
        _enemyTag = enemyTag;
    }

    private void Start()
    {
        lr.enabled = false;
        CanFire();
    }

    private Vector3 CastRay()
    {
        //TODO передалать для игрока
        var fwd = myTransform.TransformDirection(Vector3.forward) * maxDistance;

        if (Physics.Raycast(myTransform.position, fwd, out var hit))
        {
            Debug.Log($"Laser hit {hit.transform.name}");
            if (hit.transform.CompareTag(_enemyTag.ToString()))
            {
                ProcessHit(hit.point, hit.transform);
            }

            return hit.point;
        }

        return myTransform.position + myTransform.forward * maxDistance;
    }

    private void ProcessHit(Vector3 hitPosition, Transform target)
    {
        target.GetComponentInParent<HealthComponent>()?.OnEntityHit(hitPosition, _weaponAttributes);
    }

    //for player
    public void FireLaser()
    {
        FireLaser(CastRay());
    }

    public void FireLaser(Vector3 targetPosition, Transform target = null)
    {
        if (canFire)
        {
            if (target != null)
                ProcessHit(targetPosition, target);
            lr.SetPosition(0, myTransform.position);
            lr.SetPosition(1, targetPosition);
            lr.enabled = true;
            canFire = false;
            Invoke(nameof(TurnOffLaser), laserOffTime);
            Invoke(nameof(CanFire), _weaponAttributes.shotDelay);
        }
    }

    private void TurnOffLaser()
    {
        lr.enabled = false;
    }

    private void CanFire() => canFire = true;
}