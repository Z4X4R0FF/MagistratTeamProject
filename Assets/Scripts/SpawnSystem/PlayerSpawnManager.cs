using UnityEngine;
using Assets.Scripts.Vehicles.Starship;
using Assets.Scripts.Vehicles.Rover;

namespace Assets.Scripts.SpawnSystem
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnTransform;
        [SerializeField] private Transform playerRoverSpawnTransform;

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

        public Rover SpawnPlayerAtDefaultPositionRover(Rover roverPrefab)
        {
            Rover rover = Instantiate(roverPrefab, playerRoverSpawnTransform.position, playerRoverSpawnTransform.rotation);
            return rover;
        }
    }
}