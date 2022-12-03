using UnityEngine;
using Assets.Scripts.Main;

namespace Assets.Scripts.Vehicles.Starship
{

    [CreateAssetMenu(fileName = "StarshipData", menuName = "Vehicle/StarshipData", order = 0)]
    public class StarshipData : VehicleData, IStarshipData
    {
        [Header("Prefab")]
        [SerializeField] private Starship prefab;

        [Header ("Starship movement")]
        [SerializeField] private float startSpeed;
        [SerializeField] private float minSpeed;
        [SerializeField] private float rotationSpeed;

        public Starship Prefab => prefab;
        public float StartSpeed => startSpeed;
        public float MinSpeed => minSpeed;
        public float RotationSpeed => rotationSpeed;
    }
}