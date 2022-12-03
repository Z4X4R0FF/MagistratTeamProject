using UnityEngine;
using Assets.Scripts.Main;

namespace Assets.Scripts.Vehicles.Rover
{

    [CreateAssetMenu(fileName = "RoverData", menuName = "Vehicle/RoverData", order = 0)]
    public class RoverData : VehicleData, IRoverData
    {
        [Header("Prefab")]
        [SerializeField] private Rover prefab;

        [Header ("Rover movement")]
        [SerializeField] private float rotationSpeed;

        public Rover Prefab => prefab;
        public float RotationSpeed => rotationSpeed;
    }
}