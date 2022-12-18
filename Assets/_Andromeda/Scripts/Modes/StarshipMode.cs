using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.InputSystem;
using Assets.Scripts.Main;
using Assets.Scripts.Vehicles.Starship;

namespace Assets.Scripts.Modes
{
    public class StarshipMode : MonoBehaviour, IMode
    {
        private const float LANDING_DISTANCE = 200f;

        private CurrentModeManager currentModeManager;
        private InputManager inputManager;
        private StarshipController starshipController;
        private ShipAttributes currentStarshipAttributes;
        private LandingMode landingMode;

        private Starship currentStarship;
        private Camera currentShipCamera;
        private Vector2 axisInput;
        private Vector2 mouseAxisInput;

        private bool isActive;

        //private Planet[] planets;
        private WorldInfo.PlanetObjectsInfo currentPlanetToLand = null;
        private bool availableToLand = false;

        public void Init()
        {
            currentModeManager = CurrentModeManager.instance;
            starshipController = ControllersManager.instance.StarshipController;
            inputManager = InputManager.instance;
            landingMode = ModesManager.instance.LandingMode;
        }

        public void Play(Starship starship, ShipAttributes shipAttributes, Camera mainCamera)
        {
            currentModeManager.ChangeCurrentMode(this);
            isActive = true;
            currentStarship = starship;
            currentStarshipAttributes = shipAttributes;
            currentStarship.transform.parent = null;
            starshipController.StartControl(starship, shipAttributes.playerStarshipMovementAttributes);
            currentShipCamera = mainCamera;
            currentShipCamera.transform.parent = starship.CameraPoint;
            currentShipCamera.transform.localPosition = Vector3.zero;
            currentShipCamera.transform.localRotation = Quaternion.identity;

            Cursor.lockState = CursorLockMode.Locked;

            //planets = solarSystem.GetPlanets();
            availableToLand = false;
            ;
            currentPlanetToLand = null;

            inputManager.SubscribeToInputEvent(InputType.Horizontal, UpdateXInput, true);
            inputManager.SubscribeToInputEvent(InputType.Vertical, UpdateYInput, true);
            inputManager.SubscribeToInputEvent(InputType.MouseHorizontal, UpdateMouseXInput, true);
            inputManager.SubscribeToInputEvent(InputType.MouseVertical, UpdateMouseYInput, true);
            inputManager.SubscribeToInputEvent(InputType.ChangeMode, Land);

            starship.staticObjectCollisionCallBack += PlayerCrashed;
        }

        public void Stop()
        {
            inputManager.UnsubscribeFromInputEvent(InputType.Horizontal, UpdateXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.Vertical, UpdateYInput);
            inputManager.UnsubscribeFromInputEvent(InputType.MouseHorizontal, UpdateMouseXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.MouseHorizontal, UpdateMouseXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.ChangeMode, Land);

            starshipController.StopControl();

            isActive = false;
        }

        public void UpdateMode()
        {
            if (!isActive)
            {
                return;
            }

            starshipController.UpdateAxis(axisInput, mouseAxisInput);
            starshipController.UpdateController();

            if (availableToLand)
            {
                CheckCurrentPlanetToLandDistance();
            }
            else
            {
                CheckPlanetsDistance();
            }
        }

        public void Land(float value)
        {
            if (availableToLand)
            {
                var landPoint = FindClosestLandPoint(currentPlanetToLand);
                landingMode.Play(currentStarship, currentStarshipAttributes, currentPlanetToLand.Planet, landPoint,
                    currentShipCamera);
                Stop();
            }
        }

        private void UpdateXInput(float value)
        {
            axisInput.x = value;

            if (Mathf.Abs(axisInput.x) < 0.01f)
            {
                axisInput.x = 0;
            }
        }

        private void UpdateYInput(float value)
        {
            axisInput.y = value;

            if (Mathf.Abs(axisInput.y) < 0.01f)
            {
                axisInput.y = 0;
            }
        }

        private void UpdateMouseXInput(float value)
        {
            mouseAxisInput.x = value;

            if (Mathf.Abs(mouseAxisInput.x) < 0.01f)
            {
                mouseAxisInput.x = 0;
            }
        }

        private void UpdateMouseYInput(float value)
        {
            mouseAxisInput.y = value;

            if (Mathf.Abs(mouseAxisInput.y) < 0.01f)
            {
                mouseAxisInput.y = 0;
            }
        }

        private void CheckPlanetsDistance()
        {
            var nearestPlanet =
                WorldInfo.Instance.planetObjectsInfos
                    .OrderBy(r => Vector3.Distance(currentStarship.transform.position, r.PlanetPosition))
                    .First();
            var distance = Vector3.Distance(currentStarship.transform.position, nearestPlanet.PlanetPosition);
            if (distance <= LANDING_DISTANCE + nearestPlanet.Planet.elevationMinMax.Max)
            {
                availableToLand = true;
                currentPlanetToLand = nearestPlanet;
                Debug.Log("LAND");
            }
            // foreach (Planet planet in planets)
            // {
            //     float distance = Vector3.Distance(currentStarship.transform.position, planet.transform.position);
            //     if (distance <= LANDING_DISTANCE + planet.Settings.radius)
            //     {
            //         availableToLand = true;
            //         currentPlanetToLand = planet;
            //         Debug.Log("LAND");
            //         return;
            //     }
            // }
        }

        private void CheckCurrentPlanetToLandDistance()
        {
            float distance =
                Vector3.Distance(currentStarship.transform.position, currentPlanetToLand.PlanetPosition);
            if (distance > LANDING_DISTANCE + currentPlanetToLand.Planet.Settings.radius)
            {
                availableToLand = false;
                ;
                currentPlanetToLand = null;
                Debug.Log("NOT LAND");
            }
        }

        private GameObject FindClosestLandPoint(WorldInfo.PlanetObjectsInfo planetObjectsInfo)
        {
            var point = planetObjectsInfo.Generator.spawnPoints
                .Where(r => planetObjectsInfo.Generator.takenPoints.All(d =>
                    d.transform.localPosition != r.transform.localPosition))
                .Where(r =>
                    WorldInfo.Instance.placedBuildings.All(b =>
                        Vector3.Distance(b.Value.transform.localPosition, r.transform.localPosition) >
                        WorldInfo.MinDistanceBetweenBuildings))
                .OrderBy(r => Vector3.Distance(currentStarship.transform.position, r.transform.position)).First();


            // float minSqrDist = float.MaxValue;
            // float sqrDist;
            // Vector3 closestPoint = points[0];
            // Vector3 relativePosition = currentStarship.transform.position - planet.transform.position;
            // foreach (Vector3 point in points)
            // {
            //     sqrDist = (point - relativePosition).sqrMagnitude;
            //     if (sqrDist < minSqrDist)
            //     {
            //         minSqrDist = sqrDist;
            //         closestPoint = point;
            //     }
            // }
            //
            // return closestPoint;
            return point;
        }

        private void PlayerCrashed()
        {
            // TODO Crash
            Debug.Log("CRASH");
            //Stop();
        }
    }
}