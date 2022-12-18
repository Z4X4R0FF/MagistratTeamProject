using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Rover
{
    public interface IRoverController : IVehicleController
    {
        void StartControl(Rover controlledStarship, PlayerRoverMovementAttributes movementAttributes);
        void StopControl();
        void UpdateAxis(Vector2 axisInput, Vector2 mouseAxisInput);
        void UpdateController();
    }
}