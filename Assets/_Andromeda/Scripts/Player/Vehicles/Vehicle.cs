using Assets.Scripts.Main;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public abstract class Vehicle : MonoBehaviour, IVehicle
    {
        [SerializeField] private Transform cameraParent;

        private float currentSpeed = 0f;

        public float CurrentSpeed
        {
            get => currentSpeed;
            set => currentSpeed = value;
        }

        public Transform CameraPoint => cameraParent;

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void Rotate(Vector3 rotation)
        {
            transform.Rotate(rotation);
        }

        public Vector3 GetForward()
        {
            return transform.forward;
        }
    }
}