using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WorldInfo : MonoBehaviourSingleton<WorldInfo>
{
    [SerializeField]
    public List<HealthComponent> aiDamageableEntities = new();
}
