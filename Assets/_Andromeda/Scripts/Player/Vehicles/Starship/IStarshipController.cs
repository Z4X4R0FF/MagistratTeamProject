using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Starship
{
    public interface IStarshipController : IVehicleController
    {
        void StartControl(Starship controlledStarship, PlayerStarshipMovementAttributes movementAttributes);
        void StopControl();
        void UpdateAxis(Vector2 axisInput, Vector2 mouseAxisInput);
        void UpdateController();
    }
}