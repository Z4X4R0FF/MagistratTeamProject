using UnityEngine;

namespace Assets.Scripts.Modes
{
    public class ModesManager : MonoBehaviour
    {
        public static ModesManager instance;

        [SerializeField] private StarshipMode starshipMode;
        [SerializeField] private PlanetaryRoverMode planetaryRoverMode;

        public StarshipMode StarshipMode => starshipMode;
        public PlanetaryRoverMode PlanetaryRoverMode => planetaryRoverMode;

        void Awake()
        {
            instance = this;
        }

        public void Init()
        {
            starshipMode = GetComponentInChildren<StarshipMode>();
            planetaryRoverMode = GetComponentInChildren<PlanetaryRoverMode>();

            starshipMode.Init();
            planetaryRoverMode.Init();
        }
    }
}