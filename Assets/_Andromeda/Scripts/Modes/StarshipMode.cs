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
        private HealthComponent healthComponent;
        private AttackComponent attackComponent;

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
            currentStarship.shipAttributes = shipAttributes;
            currentStarship.transform.parent = null;

            healthComponent = currentStarship.GetComponent<HealthComponent>();
            healthComponent.Init(shipAttributes.healthAttributes);
            attackComponent = currentStarship.GetComponent<AttackComponent>();
            attackComponent.Init(shipAttributes.weaponAttributes, shipAttributes.enemyTag);

            starshipController.StartControl(starship, shipAttributes.playerStarshipMovementAttributes);

            currentShipCamera = mainCamera;
            currentShipCamera.transform.parent = starship.CameraPoint;
            currentShipCamera.transform.localPosition = Vector3.zero;
            currentShipCamera.transform.localRotation = Quaternion.identity;

            Cursor.lockState = CursorLockMode.Locked;

            availableToLand = false;
            currentPlanetToLand = null;
            

            inputManager.SubscribeToInputEvent(InputType.Horizontal, UpdateXInput, true);
            inputManager.SubscribeToInputEvent(InputType.Vertical, UpdateYInput, true);
            inputManager.SubscribeToInputEvent(InputType.MouseHorizontal, UpdateMouseXInput, true);
            inputManager.SubscribeToInputEvent(InputType.MouseVertical, UpdateMouseYInput, true);
            inputManager.SubscribeToInputEvent(InputType.ChangeMode, Land);
            inputManager.SubscribeToInputEvent(InputType.Attack, PlayerFire);

            currentStarship.staticObjectCollisionCallback += DamagePlayer;
            currentStarship.planetSurfaceCollisionCallback += PlayerCrushed;
        }

        public void Stop()
        {
            inputManager.UnsubscribeFromInputEvent(InputType.Horizontal, UpdateXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.Vertical, UpdateYInput);
            inputManager.UnsubscribeFromInputEvent(InputType.MouseHorizontal, UpdateMouseXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.MouseHorizontal, UpdateMouseXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.ChangeMode, Land);
            inputManager.UnsubscribeFromInputEvent(InputType.Attack, PlayerFire);

            currentStarship.staticObjectCollisionCallback -= DamagePlayer;
            currentStarship.planetSurfaceCollisionCallback -= PlayerCrushed;
            

            starshipController.StopControl();
            healthComponent.onEntityDestroyed.Invoke(healthComponent);

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
                LandingHint.Instance.DisableHint();
                Vector3 position = FindClosestLandPoint(currentPlanetToLand);
                Quaternion rotation =
                    Quaternion.LookRotation(position - currentPlanetToLand.Planet.transform.position, Vector3.up) *
                    Quaternion.Euler(90, 0, 0);

                if (position.Equals(Vector3.zero))
                {
                    Debug.Log("Zero land vector");
                    GameObject landingPoint = CalculateClosestLandPoint(currentPlanetToLand);
                    position = landingPoint.transform.position;
                    rotation = landingPoint.transform.rotation;
                }

                landingMode.Play(currentStarship, currentPlanetToLand, position, rotation, currentShipCamera);
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

        private void PlayerFire(float value)
        {
            attackComponent.PlayerFire();
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
                LandingHint.Instance.OnEnableLand();
                Debug.Log("LAND");
            }
        }

        private void CheckCurrentPlanetToLandDistance()
        {
            float distance =
                Vector3.Distance(currentStarship.transform.position, currentPlanetToLand.PlanetPosition);
            if (distance > LANDING_DISTANCE + currentPlanetToLand.Planet.elevationMinMax.Max)
            {
                availableToLand = false;
                currentPlanetToLand = null;
                LandingHint.Instance.DisableHint();
                Debug.Log("NOT LAND");
            }
        }

        private GameObject CalculateClosestLandPoint(WorldInfo.PlanetObjectsInfo planetObjectsInfo)
        {
            var point = planetObjectsInfo.Generator.spawnPoints
                .Where(r => planetObjectsInfo.Generator.takenPoints.All(d =>
                    d.transform.localPosition != r.transform.localPosition))
                .Where(r =>
                    WorldInfo.Instance.placedBuildings.All(b =>
                        (b.Value.transform.localPosition - r.transform.localPosition).sqrMagnitude >
                        WorldInfo.MinDistanceBetweenBuildings * WorldInfo.MinDistanceBetweenBuildings))
                .OrderBy(r => Vector3.Distance(currentStarship.transform.position, r.transform.position)).First();
            return point;
        }


        private Vector3 FindClosestLandPoint(WorldInfo.PlanetObjectsInfo planetObjectsInfo)
        {
            Vector3 direction = planetObjectsInfo.Planet.transform.position - currentStarship.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(currentStarship.transform.position, direction, out hit,
                    planetObjectsInfo.Planet.elevationMinMax.Max + 200f, 64))
            {
                Debug.DrawRay(currentStarship.transform.position, direction, Color.yellow, 30f);
                return hit.point;
            }

            Debug.DrawRay(currentStarship.transform.position, direction, Color.red, 30f);

            Debug.Log("Raycast cant find closest land point");
            return Vector3.zero;
        }

        private void PlayerCrushed()
        {
            // TODO Crash
            healthComponent.OnEntityHitSurface();
            Debug.Log("CRASH");
            Stop();
        }

        private void DamagePlayer()
        {
            //TODO damage
            Debug.Log("Damage");
        }
    }
}