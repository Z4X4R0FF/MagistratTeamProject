using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Rover
{
    public class RoverController : MonoBehaviour, IRoverController
    {
        private Rover rover;
        private PlayerRoverMovementAttributes roverMovementAttributes;
        private WorldInfo.PlanetObjectsInfo currentPlanetInfo;

        private Vector2 axisInput;
        private Vector2 mouseAxisInput;
        private bool isActive = false;

        private RaycastHit hit;

        public void StartControl(Rover controlledRover, PlayerRoverMovementAttributes movementAttributes,
            WorldInfo.PlanetObjectsInfo planetInfo)
        {
            rover = controlledRover;
            roverMovementAttributes = movementAttributes;
            currentPlanetInfo = planetInfo;
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

            var allowedAcceleration = Math.Clamp(axisInput.y, 0f, 1f);
            Move(allowedAcceleration);
            PlayerHealthDisplay.Instance.UpdatePlayerStat(PlayerHealthDisplay.PlayerStat.Speed,
                allowedAcceleration * 10, 10);
            Rotate(new Vector3(0, axisInput.x, 0));
        }

        private void Move(float moveValue)
        {
            if (moveValue == 0f)
            {
                return;
            }

            Vector3 planetPosition = currentPlanetInfo.Planet.transform.position;

            Vector3 nextPos = rover.transform.position +
                              rover.transform.forward * moveValue * roverMovementAttributes.maxSpeed * Time.deltaTime;

            Vector3 direction = planetPosition - nextPos;

            if (Physics.Raycast(nextPos, direction, out hit, currentPlanetInfo.Planet.elevationMinMax.Max + 200f, 64))
            {
                Debug.DrawRay(nextPos, direction, Color.yellow, 2f);
                Vector3 newPosition = hit.point + (hit.point - planetPosition).normalized;

                var angle = Vector3.Angle(rover.transform.position - planetPosition, newPosition - planetPosition);

                rover.transform.position = newPosition;
                rover.transform.Rotate(new Vector3(angle, 0, 0));
            }
            else
            {
                Debug.DrawRay(nextPos, direction, Color.red, 2f);
                Debug.Log("No raycast");
            }
        }

        public void Rotate(Vector3 rotation)
        {
            rover.Rotate(new Vector3(0, rotation.y, 0) * roverMovementAttributes.rotationSpeed * Time.deltaTime);
        }
    }
}