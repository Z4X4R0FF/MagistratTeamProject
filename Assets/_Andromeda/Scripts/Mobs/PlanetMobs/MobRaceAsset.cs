using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MobRaceAsset : ScriptableObject
{
    public string RaceName;
    public GameObject SpawnerPrefab;

    [Tooltip("Will be multiplied by planet radius/10")]
    public int minSpawnerCount;

    public int maxSpawnerCount;
}