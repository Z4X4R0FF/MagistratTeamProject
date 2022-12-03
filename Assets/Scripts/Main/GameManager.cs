using UnityEngine;
using Assets.Scripts.SpawnSystem;
using Assets.Scripts.Vehicles.Starship;
using Assets.Scripts.Vehicles.Rover;
using Assets.Scripts.Modes;

namespace Assets.Scripts.Main
{
    public class GameManager : MonoBehaviour
    {
        private const string START_STARSHIP_NAME = "Starship";
        private const string START_ROVER_NAME = "Rover";

        private Camera mainCamera;
        private DatabaseManager databaseManager;
        private ModesManager modesManager;
        private CurrentModeManager currentModeManager;
        [SerializeField] private bool useRoverStart = false;

        void Start()
        {
            mainCamera = Camera.main;
            databaseManager = DatabaseManager.instanse;
            modesManager = ModesManager.instance;
            currentModeManager = CurrentModeManager.instance;

            modesManager.Init();

            if (!useRoverStart)
            {
                StarshipData starshipData = (StarshipData)databaseManager.VehicleDatabase.GetData(START_STARSHIP_NAME);
                Starship starship = PlayerSpawnManager.instance.SpawnPlayerAtDefaultPosition(starshipData.Prefab);
                modesManager.StarshipMode.Play(starship, starshipData, mainCamera);
            }
            else
            {
                RoverData roverData = (RoverData)databaseManager.VehicleDatabase.GetData(START_ROVER_NAME);
                Rover rover = PlayerSpawnManager.instance.SpawnPlayerAtDefaultPositionRover(roverData.Prefab);
                modesManager.PlanetaryRoverMode.Play(rover, roverData, mainCamera);
            }
        }

        private void Update()
        {
            currentModeManager.UpdateCurrentMode();
        }
    }
}