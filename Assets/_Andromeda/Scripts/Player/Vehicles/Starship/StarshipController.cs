using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Starship
{
    public class StarshipController : MonoBehaviour, IStarshipController
    {
        private Starship starship;
        private PlayerStarshipMovementAttributes starshipMovementAttributes;

        private Vector2 axisInput;
        private Vector2 mouseAxisInput;
        private bool isActive = false;

        public void StartControl(Starship controlledStarship, PlayerStarshipMovementAttributes movementAttributes)
        {
            starship = controlledStarship;
            starshipMovementAttributes = movementAttributes;
            isActive = true;
            starship.CurrentSpeed = starshipMovementAttributes.startSpeed;
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

            Rotate(new Vector3(-mouseAxisInput.y, mouseAxisInput.x, -axisInput.x));

            if (axisInput.y > 0)
            {
                Accelerate();
            }

            if (axisInput.y < 0)
            {
                SlowDown();
            }
            Move();
        }

        private void Move()
        {
            starship.SetPosition(starship.GetPosition() + starship.GetForward() * starship.CurrentSpeed * Time.deltaTime);
        }

        public void Rotate(Vector3 rotation)
        {
            starship.Rotate(rotation * starshipMovementAttributes.rotationSpeed * Time.deltaTime);
        }

        private void Accelerate()
        {
            starship.CurrentSpeed = Mathf.Lerp(starship.CurrentSpeed, starshipMovementAttributes.maxSpeed, starshipMovementAttributes.accelerateSpeed * Time.deltaTime);
        }

        private void SlowDown()
        {
            starship.CurrentSpeed = Mathf.Lerp(starship.CurrentSpeed, starshipMovementAttributes.minSpeed, starshipMovementAttributes.slowDownSpeed * Time.deltaTime);
        }
    }
}