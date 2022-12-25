using UnityEngine;
using Assets.Scripts.InputSystem;
using Assets.Scripts.Main;
using Assets.Scripts.Vehicles.Rover;
using Assets.Scripts.Vehicles.Starship;

namespace Assets.Scripts.Modes
{
    public class PlanetaryRoverMode : MonoBehaviour, IMode
    {
        private CurrentModeManager currentModeManager;
        private InputManager inputManager;
        private RoverController roverController;
        private TakeOffMode takeOffMode;

        private const float DISTANCE_TO_STARSHIP = 15f;

        private Starship currentStarship;
        private Rover currentRover;
        private WorldInfo.PlanetObjectsInfo currentPlanetinfo;
        private Camera currentCamera;
        private Vector2 axisInput;
        private Vector2 mouseAxisInput;
        private bool isActive;

        public void Init()
        {
            currentModeManager = CurrentModeManager.instance;
            roverController = ControllersManager.instance.RoverController;
            inputManager = InputManager.instance;
            takeOffMode = ModesManager.instance.TakeOffMode;
        }

        public void Play(Starship starship, Rover rover, RoverAttributes roverAttributes, WorldInfo.PlanetObjectsInfo planetInfo, Camera mainCamera)
        {
            currentModeManager.ChangeCurrentMode(this);
            isActive = true;
            roverController.StartControl(rover, roverAttributes.playerRoverMovementAttributes, planetInfo);
            currentRover = rover;
            currentStarship = starship;
            currentPlanetinfo = planetInfo;
            currentCamera = mainCamera;
            mainCamera.transform.parent = rover.CameraPoint;
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.transform.localRotation = Quaternion.identity;

            inputManager.SubscribeToInputEvent(InputType.Horizontal, UpdateXInput, true);
            inputManager.SubscribeToInputEvent(InputType.Vertical, UpdateYInput, true);
            inputManager.SubscribeToInputEvent(InputType.MouseHorizontal, UpdateMouseXInput, true);
            inputManager.SubscribeToInputEvent(InputType.MouseVertical, UpdateMouseYInput, true);
            inputManager.SubscribeToInputEvent(InputType.ChangeMode, TakeOff);
        }

        public void Stop()
        {
            inputManager.UnsubscribeFromInputEvent(InputType.Horizontal, UpdateXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.Vertical, UpdateYInput);
            inputManager.UnsubscribeFromInputEvent(InputType.MouseHorizontal, UpdateMouseXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.MouseHorizontal, UpdateMouseXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.ChangeMode, TakeOff);

            roverController.StopControl();
            Destroy(currentRover);

            isActive = false;
        }

        private void TakeOff(float value)
        {
            if (IsNearStarship())
            {
                takeOffMode.Play(currentStarship, currentStarship.shipAttributes, currentPlanetinfo, currentCamera);
                Stop();
            }
        }

        public void UpdateMode()
        {
            if (!isActive)
            {
                return;
            }

            roverController.UpdateAxis(axisInput, mouseAxisInput);
            roverController.UpdateController();
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

        private bool IsNearStarship()
        {
            float sqrDistance = (currentRover.transform.position - currentStarship.transform.position).sqrMagnitude;

            return sqrDistance <= DISTANCE_TO_STARSHIP * DISTANCE_TO_STARSHIP;
        }
    }
}