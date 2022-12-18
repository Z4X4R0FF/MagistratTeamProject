using Assets.Scripts.Main;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Starship
{
    public class Starship : Vehicle, IStarship
    {
        public delegate void StaticObjectCollision();
        public StaticObjectCollision staticObjectCollisionCallBack;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "StaticObject" || other.tag == "PlanetSurface")
            {
                if(staticObjectCollisionCallBack != null)
                {
                    staticObjectCollisionCallBack.Invoke();
                }
            }
        }
    }
}