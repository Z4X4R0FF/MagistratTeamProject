using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        if (Camera.main != null) mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
            transform.LookAt(mainCamera);
    }
}