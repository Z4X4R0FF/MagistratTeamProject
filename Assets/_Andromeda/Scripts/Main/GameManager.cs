using UnityEngine;
using Assets.Scripts.SpawnSystem;
using Assets.Scripts.Vehicles.Starship;
using Assets.Scripts.Vehicles.Rover;
using Assets.Scripts.Modes;

namespace Assets.Scripts.Main
{
    public class GameManager : MonoBehaviour
    {
        private Camera mainCamera;
        private DatabaseManager databaseManager;
        private ModesManager modesManager;
        private CurrentModeManager currentModeManager;
        private SolarSystem solarSystem;

        void Start()
        {
            mainCamera = Camera.main;
            databaseManager = DatabaseManager.instanse;
            modesManager = ModesManager.instance;
            currentModeManager = CurrentModeManager.instance;
            solarSystem = SolarSystem.Instance;

            modesManager.Init();

            solarSystem.GenerateSystem();

            ShipAttributes playerStarshipAttributes = databaseManager.PlayerStarship;
            Starship starship = PlayerSpawnManager.instance.SpawnPlayerAtDefaultPosition(playerStarshipAttributes.prefab);
            modesManager.StarshipMode.Play(starship, playerStarshipAttributes, mainCamera);
        }

        private void Update()
        {
            currentModeManager.UpdateCurrentMode();
        }
    }
}