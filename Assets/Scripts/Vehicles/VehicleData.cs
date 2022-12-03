using UnityEngine;
using Assets.Scripts.Main;

namespace Assets.Scripts.Vehicles
{
    public class VehicleData : EntityData, IVehicleData
    {
        [Header("Vehicle movement")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float accelerateSpeed;
        [SerializeField] private float slowDownSpeed;

        public float MaxSpeed => maxSpeed;
        public float AccelerateSpeed => accelerateSpeed;
        public float SlowDownSpeed => slowDownSpeed;
    }
}