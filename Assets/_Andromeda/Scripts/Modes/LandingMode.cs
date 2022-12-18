using UnityEngine;
using Assets.Scripts.Vehicles.Starship;

namespace Assets.Scripts.Modes
{
    public class LandingMode : MonoBehaviour, IMode
    {
        private CurrentModeManager currentModeManager;
        private TakeOffMode takeOffMode; //временно

        private const float LANDING_SPEED = 0.25f;

        private Starship currentStarship;
        private ShipAttributes currentStarshipAttributes;
        private bool isActive;
        private Planet currentPlanet;
        private Camera currentShipCamera;
        private float landingProgress = 0f;

        private Vector3 startPosition;
        private Vector3 landingPosition;
        private Vector3 startRotation;
        private Vector3 landingRotation;

        public void Init()
        {
            currentModeManager = CurrentModeManager.instance;
            takeOffMode = ModesManager.instance.TakeOffMode; //временно
        }

        public void Play(Starship starship, ShipAttributes shipAttributes, Planet planet, Vector3 positionToLand, Camera mainCamera)
        {
            Debug.Log("Start landing");
            currentModeManager.ChangeCurrentMode(this);
            isActive = true;
            currentStarship = starship;
            currentStarship.transform.parent = planet.transform;
            currentStarshipAttributes = shipAttributes;
            currentPlanet = planet;

            currentShipCamera = mainCamera;
            currentShipCamera.transform.parent = starship.CameraPoint;
            currentShipCamera.transform.localPosition = Vector3.zero;
            currentShipCamera.transform.localRotation = Quaternion.identity;

            Cursor.lockState = CursorLockMode.Locked;

            landingProgress = 0.0f;
            startPosition = starship.transform.localPosition;
            landingPosition = positionToLand;
            startRotation = starship.transform.localRotation.eulerAngles;
            landingRotation = (Quaternion.LookRotation(landingPosition, Vector3.down) * Quaternion.Euler(90, 0, 0)).eulerAngles;
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
            landingProgress += LANDING_SPEED * Time.deltaTime;
            currentStarship.transform.localPosition = Vector3.Lerp(startPosition, landingPosition, landingProgress);
            currentStarship.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRotation, landingRotation, landingProgress));
            if(landingProgress >= 1)
            {
                FinishLanding();
            }
        }

        private void FinishLanding()
        {
            Debug.Log("Stop landing");
            takeOffMode.Play(currentStarship, currentStarshipAttributes, currentPlanet, currentShipCamera);
            Stop();
        }
    }
}