using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlanetPropsSettingsAsset : ScriptableObject
{
    [Tooltip("Will be multiplied by planet radius/10")]
    public int minPropSpawnCount;

    public int maxPropSpawnCount;
    public List<GameObject> planetProps;
}