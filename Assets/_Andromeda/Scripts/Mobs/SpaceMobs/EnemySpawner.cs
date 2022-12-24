using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTimer = 1f;

    private void Start()
    {
        SpawnEnemy();
        //StartSpawning();
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, Random.onUnitSphere + transform.position, Quaternion.identity);
    }

    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnTimer, spawnTimer);
    }

    public void StopSpawning() => CancelInvoke();
}