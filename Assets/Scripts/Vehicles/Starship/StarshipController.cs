using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Starship
{
    public class StarshipController : MonoBehaviour, IStarshipController
    {
        private Starship starship;
        private StarshipData starshipData;

        private Vector2 axisInput;
        private Vector2 mouseAxisInput;
        private bool isActive = false;

        public void StartControl(Starship controlledStarship, StarshipData controlledStarshipData)
        {
            starship = controlledStarship;
            starshipData = controlledStarshipData;
            isActive = true;
            starship.CurrentSpeed = starshipData.StartSpeed;
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
            starship.Rotate(rotation * starshipData.RotationSpeed * Time.deltaTime);
        }

        private void Accelerate()
        {
            starship.CurrentSpeed = Mathf.Lerp(starship.CurrentSpeed, starshipData.MaxSpeed, starshipData.AccelerateSpeed * Time.deltaTime);
        }

        private void SlowDown()
        {
            starship.CurrentSpeed = Mathf.Lerp(starship.CurrentSpeed, starshipData.MinSpeed, starshipData.SlowDownSpeed * Time.deltaTime);
        }
    }
}