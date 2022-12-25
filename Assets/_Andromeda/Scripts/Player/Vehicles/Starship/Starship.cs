using Assets.Scripts.Main;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Starship
{
    public class Starship : Vehicle, IStarship
    {
        public delegate void StaticObjectCollision();
        public StaticObjectCollision staticObjectCollisionCallback;
        public StaticObjectCollision planetSurfaceCollisionCallback;

        public ShipAttributes shipAttributes;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "StaticObject" && staticObjectCollisionCallback != null)
            {
                staticObjectCollisionCallback.Invoke();
            }
            if(other.tag == "PlanetSurface" && planetSurfaceCollisionCallback != null)
            {
                planetSurfaceCollisionCallback.Invoke();
            }
        }
    }
}