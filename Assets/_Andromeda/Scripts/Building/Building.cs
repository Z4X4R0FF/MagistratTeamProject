using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject hitExplosion;
    [SerializeField] private GameObject destroyExplosion;

    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    public void OnBuildingHit(Vector3 pos)
    {
        var go = Instantiate(hitExplosion, pos, Quaternion.identity, myTransform);
        Destroy(go, 2);
    }
}