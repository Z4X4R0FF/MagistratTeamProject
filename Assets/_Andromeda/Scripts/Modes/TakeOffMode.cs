using UnityEngine;
using Assets.Scripts.Vehicles.Starship;

namespace Assets.Scripts.Modes
{
    public class TakeOffMode : MonoBehaviour, IMode
    {
        private CurrentModeManager currentModeManager;
        private StarshipMode starshipMode;

        private const float TAKING_OFF_SPEED = 0.25f;
        private const float TAKING_OFF_DISTANCE = 100f;

        private Starship currentStarship;
        private ShipAttributes currentStarshipAttributes;
        private bool isActive;
        private WorldInfo.PlanetObjectsInfo currentPlanetInfo;
        private Camera currentShipCamera;
        private float takingOffProgress = 0f;

        private Vector3 startPosition;
        private Vector3 takingOffPosition;

        public void Init()
        {
            currentModeManager = CurrentModeManager.instance;
            starshipMode = ModesManager.instance.StarshipMode;
        }

        public void Play(Starship starship, ShipAttributes shipAttributes, WorldInfo.PlanetObjectsInfo planetInfo, Camera mainCamera)
        {
            Debug.Log("Start taking off");
            currentModeManager.ChangeCurrentMode(this);
            isActive = true;
            currentStarship = starship;
            currentStarship.transform.parent = planetInfo.Planet.transform;
            currentStarshipAttributes = shipAttributes;
            currentPlanetInfo = planetInfo;

            currentShipCamera = mainCamera;
            currentShipCamera.transform.parent = starship.CameraPoint;
            currentShipCamera.transform.localPosition = Vector3.zero;
            currentShipCamera.transform.localRotation = Quaternion.identity;

            Cursor.lockState = CursorLockMode.Locked;

            takingOffProgress = 0.0f;
            startPosition = starship.transform.localPosition;
            takingOffPosition = startPosition + startPosition.normalized * TAKING_OFF_DISTANCE;
        }

        public void Stop()
        {
            isActive = false;
        }

        public void UpdateMode()
        {
            if (!isActive)
            {
                return;
            }
            takingOffProgress += TAKING_OFF_SPEED * Time.deltaTime;
            currentStarship.transform.localPosition = Vector3.Lerp(startPosition, takingOffPosition, takingOffProgress);

            if(takingOffProgress >= 1)
            {
                FinishTakingOff();
            }
        }

        private void FinishTakingOff()
        {
            Debug.Log("Stop taking off");
            starshipMode.Play(currentStarship, currentStarshipAttributes, currentShipCamera);
            Stop();
        }
    }
}