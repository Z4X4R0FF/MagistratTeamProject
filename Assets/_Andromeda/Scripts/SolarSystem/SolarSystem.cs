using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SolarSystem : MonoBehaviour
public class SolarSystem : MonoBehaviourSingleton<SolarSystem>
{
    [SerializeField] private int planetCount;
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private int maxOffsetX;
    [SerializeField] private int minOffsetX;
    [SerializeField] private int minDistanceBetweenPlanetOrbits;
    [SerializeField] private List<PlanetGenerationSettingsAsset> planetPresets;

    private readonly List<int> _planetOrbitsOffsets = new();
    private GameObject[] _planets;

    public void GenerateSystem()
    {
        _planets = null;

        if (_planets == null || _planets.Length == 0)
        {
            _planets = new GameObject[planetCount];
        }

        for (var i = 0; i < _planets.Length; i++)
        {
            var planetOffsetX = Random.Range(minOffsetX, maxOffsetX);
            var flag = 0;
            while (_planetOrbitsOffsets.Any(r =>
                       planetOffsetX > r - minDistanceBetweenPlanetOrbits &&
                       planetOffsetX < r + minDistanceBetweenPlanetOrbits))
            {
                planetOffsetX = Random.Range(minOffsetX, maxOffsetX);
                flag++;
                if (flag > 1000)
                {
                    Debug.LogWarning("while iterations limit exceeded");
                    break;
                }
            }

            _planetOrbitsOffsets.Add(planetOffsetX);
            if (_planets[i] == null) _planets[i] = Instantiate(planetPrefab, transform, true);
            _planets[i].GetComponentInChildren<Planet>()
                .GeneratePlanet(planetOffsetX, planetPresets[Random.Range(0, planetPresets.Count)]);
            //SetPlanetOrbitAndRotation(i);
        }
    }

    private void SetPlanetOrbitAndRotation(int planetIndex)
    {
        //initial rotation and orbit
        _planets[planetIndex].transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        var planetRot = _planets[planetIndex].GetComponentInChildren<Planet>().transform.rotation = Random.rotation;

        // orbit speed
        var orbitSpeed = Random.Range(0.5f, 5);
        _planets[planetIndex].transform.DORotate(
                _planets[planetIndex].transform.rotation.eulerAngles + new Vector3(0, orbitSpeed, 0), 1f,
                RotateMode.Fast)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        //self rotation speed 
        var selfRotation = Random.Range(5, 15);
        _planets[planetIndex].GetComponentInChildren<Planet>().transform
            .DORotate(planetRot.eulerAngles + new Vector3(0, selfRotation, 0), 1f, RotateMode.Fast)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    public Planet[] GetPlanets()
    {
        return _planets;
    }
}