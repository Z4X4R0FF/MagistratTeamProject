using UnityEngine;
using Assets.Scripts.Main;

namespace Assets.Scripts.Vehicles
{
    [CreateAssetMenu(fileName = "VehicleDatabase", menuName = "Databases/VehicleDatabase", order = 0)]
    public class VehicleDatabase : Database<VehicleData>
    {
    }
}