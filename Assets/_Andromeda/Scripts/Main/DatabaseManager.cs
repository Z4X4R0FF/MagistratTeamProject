using UnityEngine;
using Assets.Scripts.Vehicles;

namespace Assets.Scripts.Main
{
    public class DatabaseManager : MonoBehaviour
    {
        public static DatabaseManager instanse;

        [SerializeField] ShipAttributes playerStarship;
        [SerializeField] RoverAttributes playerRover;

        public ShipAttributes PlayerStarship => playerStarship;
        public RoverAttributes PlayerRover => playerRover;

        private void Awake()
        {
            instanse = this;
        }
    }
}