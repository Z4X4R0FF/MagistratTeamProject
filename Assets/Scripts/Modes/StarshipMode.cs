using UnityEngine;
using Assets.Scripts.InputSystem;
using Assets.Scripts.Main;
using Assets.Scripts.Vehicles.Starship;

namespace Assets.Scripts.Modes
{
    public class StarshipMode : MonoBehaviour, IMode
    {
        private CurrentModeManager currentModeManager;
        private InputManager inputManager;
        private StarshipController starshipController;

        private Vector2 axisInput;
        private Vector2 mouseAxisInput;
        private bool isActive;

        public void Init()
        {
            currentModeManager = CurrentModeManager.instance;
            starshipController = ControllersManager.instance.StarshipController;
            inputManager = InputManager.instance;
        }

        public void Play(Starship starship, StarshipData starshipData, Camera mainCamera)
        {
            currentModeManager.ChangeCurrentMode(this);
            isActive = true;
            starshipController.StartControl(starship, starshipData);
            mainCamera.transform.parent = starship.CameraPoint;
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.transform.localRotation = Quaternion.identity;

            inputManager.SubscribeToInputEvent(InputType.Horizontal, UpdateXInput, true);
            inputManager.SubscribeToInputEvent(InputType.Vertical, UpdateYInput, true);
            inputManager.SubscribeToInputEvent(InputType.MouseHorizontal, UpdateMouseXInput, true);
            inputManager.SubscribeToInputEvent(InputType.MouseVertical, UpdateMouseYInput, true);
        }

        public void Stop()
        {
            inputManager.UnsubscribeFromInputEvent(InputType.Horizontal, UpdateXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.Vertical, UpdateYInput);
            inputManager.UnsubscribeFromInputEvent(InputType.MouseHorizontal, UpdateMouseXInput);
            inputManager.UnsubscribeFromInputEvent(InputType.MouseHorizontal, UpdateMouseXInput);

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
    }
}