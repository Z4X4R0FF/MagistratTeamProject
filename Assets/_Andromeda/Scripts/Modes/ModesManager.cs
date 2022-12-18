using UnityEngine;

namespace Assets.Scripts.Modes
{
    public class ModesManager : MonoBehaviour
    {
        public static ModesManager instance;

        [SerializeField] private StarshipMode starshipMode;
        [SerializeField] private PlanetaryRoverMode planetaryRoverMode;
        [SerializeField] private LandingMode landingMode;
        [SerializeField] private TakeOffMode takeOffMode;

        public StarshipMode StarshipMode => starshipMode;
        public PlanetaryRoverMode PlanetaryRoverMode => planetaryRoverMode;
        public LandingMode LandingMode => landingMode;
        public TakeOffMode TakeOffMode => takeOffMode;

        void Awake()
        {
            instance = this;
        }

        public void Init()
        {
            starshipMode = GetComponentInChildren<StarshipMode>();
            planetaryRoverMode = GetComponentInChildren<PlanetaryRoverMode>();
            landingMode = GetComponentInChildren<LandingMode>();
            takeOffMode = GetComponentInChildren<TakeOffMode>();

            starshipMode.Init();
            planetaryRoverMode.Init();
            landingMode.Init();
            takeOffMode.Init();
        }
    }
}