using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    [SerializeField] private TrailRenderer tr;

    public void Activate(bool activate = true)
    {
        tr.enabled = activate;
    }
}