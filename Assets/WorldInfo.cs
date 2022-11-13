using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WorldInfo : MonoBehaviourSingleton<WorldInfo>
{
    [SerializeField]
    public List<GameObject> AiTargets = new();
}
