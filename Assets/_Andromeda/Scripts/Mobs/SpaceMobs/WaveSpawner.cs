using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviourSingleton<WaveSpawner>
{
    [SerializeField] private RectTransform waveInfo;
    [SerializeField] private Text waveText;

    [SerializeField] private float systemRadius;
    [SerializeField] private float spawnRadius;
    [SerializeField] private List<Wave> waves;

    private float timeLeft;
    private bool timerOn;

    private const string Text = "Атака пиратов через ";
    private Wave currWave;

    private void Start()
    {
        waveInfo.gameObject.SetActive(false);
        currWave = GetRandomWave();
        timeLeft = currWave.awareTime;
    }

    public void EnableWaves()
    {
        waveInfo.gameObject.SetActive(true);
        timerOn = true;
    }

    private void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                StartCoroutine(SpawnWave(currWave));
                currWave = waves[Random.Range(0, waves.Count)];
                timeLeft = currWave.awareTime;
            }
        }
    }

    private Wave GetRandomWave() => waves[Random.Range(0, waves.Count)];

    private IEnumerator SpawnWave(Wave wave)
    {
        var spawnPos = Random.insideUnitCircle * systemRadius;
        for (var i = 0; i < wave.enemies.Count; i++)
        {
            for (var j = 0; j < wave.baseCount[i]; j++)
            {
                Instantiate(wave.enemies[i],
                    new Vector3(spawnPos.x + Random.Range(-spawnRadius, spawnRadius),
                        Random.Range(-spawnRadius, spawnRadius), spawnPos.y + Random.Range(-spawnRadius, spawnRadius)),
                    quaternion.identity);
                yield return null;
            }
        }
    }

    private void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        var minutes = Mathf.FloorToInt(currentTime / 60);
        var seconds = Mathf.FloorToInt(currentTime % 60);

        waveText.text = Text + $"{minutes:00}:{seconds:00}";
    }

    [Serializable]
    public class Wave
    {
        public float awareTime;
        public List<GameObject> enemies;
        public List<int> baseCount;
    }
}