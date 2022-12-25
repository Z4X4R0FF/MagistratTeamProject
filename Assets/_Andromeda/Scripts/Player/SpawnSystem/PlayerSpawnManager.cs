using UnityEngine;
using Assets.Scripts.Vehicles.Starship;
using Assets.Scripts.Vehicles.Rover;

namespace Assets.Scripts.SpawnSystem
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnTransform;

        public static PlayerSpawnManager instance;

        private void Awake()
        {
            instance = this;
        }

        public Starship SpawnPlayerAtDefaultPosition(Starship starshipPrefab)
        {
            Starship starship = Instantiate(starshipPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation);
            return starship;
        }

        public Rover SpawnRover(Rover roverPrefab, Planet parentPlanet, Vector3 position, Quaternion rotation)
        {
            Rover rover = Instantiate(roverPrefab, position, rotation, parentPlanet.transform);
            return rover;
        }
    }
}