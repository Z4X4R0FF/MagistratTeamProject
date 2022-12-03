using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Rover
{
    public class RoverController : MonoBehaviour, IRoverController
    {
        private Rover rover;
        private RoverData roverData;

        private Vector2 axisInput;
        private Vector2 mouseAxisInput;
        private bool isActive = false;

        public void StartControl(Rover controlledRover, RoverData controlledRoverData)
        {
            rover = controlledRover;
            roverData = controlledRoverData;
            isActive = true;
        }

        public void StopControl()
        {
            isActive = false;
        }

        public void UpdateAxis(Vector2 axisInput, Vector2 mouseAxisInput)
        {
            this.axisInput = axisInput;
            this.mouseAxisInput = mouseAxisInput;
        }

        public void UpdateController()
        {
            if (!isActive)
            {
                return;
            }

            Rotate(new Vector3(0, axisInput.x, 0));
            Move(axisInput.y);
        }

        private void Move(float moveValue)
        {
            rover.SetPosition(rover.GetPosition() + rover.GetForward() * moveValue * roverData.MaxSpeed * Time.deltaTime);
        }

        public void Rotate(Vector3 rotation)
        {
            rover.Rotate(new Vector3(0, rotation.y, 0) * roverData.RotationSpeed * Time.deltaTime);
        }
    }
}