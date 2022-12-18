using UnityEngine;
using Assets.Scripts.Vehicles.Starship;
using Assets.Scripts.Vehicles.Rover;

namespace Assets.Scripts.Main
{
    public class ControllersManager : MonoBehaviour
    {
        [SerializeField] private StarshipController starshipController;
        [SerializeField] private RoverController roverController;

        public static ControllersManager instance;

        public StarshipController StarshipController => starshipController;
        public RoverController RoverController => roverController;

        private void Awake()
        {
            instance = this;
        }
    }
}