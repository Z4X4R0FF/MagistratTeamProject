using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlanetColorSettingsAsset : ScriptableObject
{
    public Gradient gradient;
    public Material material;
    
    private void OnValidate()
    {
        Validated?.Invoke();
    }

    public event Action Validated;
}
