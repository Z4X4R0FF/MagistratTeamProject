using UnityEngine;
using Assets.Scripts.Vehicles;

namespace Assets.Scripts.Main
{
    public class DatabaseManager : MonoBehaviour
    {
        public static DatabaseManager instanse;

        [SerializeField] VehicleDatabase vehicleDatabase;

        public VehicleDatabase VehicleDatabase => vehicleDatabase;

        private void Awake()
        {
            instanse = this;
        }
    }
}