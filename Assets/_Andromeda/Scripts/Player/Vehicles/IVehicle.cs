using UnityEngine;
using Assets.Scripts.Main;

namespace Assets.Scripts.Vehicles
{
    public interface IVehicle
    {
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        Vector3 GetPosition();
        Quaternion GetRotation();
    }
}