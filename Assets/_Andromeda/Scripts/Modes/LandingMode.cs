using UnityEngine;
using Assets.Scripts.Vehicles.Starship;
using Assets.Scripts.Main;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Modes
{
    public class LandingMode : MonoBehaviour, IMode
    {
        private CurrentModeManager currentModeManager;

        private const float LANDING_SPEED = 0.25f;
        private float ROVER_SPAWN_DISTANCE = 15f;

        private Starship currentStarship;
        private ShipAttributes currentStarshipAttributes;
        private bool isActive;
        private WorldInfo.PlanetObjectsInfo currentPlanetInfo;
        private Camera currentShipCamera;

        private Vector3 landingPosition;
        private Quaternion landingRotation;

        public void Init()
        {
            currentModeManager = CurrentModeManager.instance;
        }

        public void Play(Starship starship, WorldInfo.PlanetObjectsInfo planetInfo, Vector3 position, Quaternion rotation,
            Camera mainCamera)
        {
            Debug.Log("Start landing");
            currentModeManager.ChangeCurrentMode(this);
            isActive = true;
            currentStarship = starship;
            currentStarship.transform.parent = planetInfo.Planet.transform;
            currentStarshipAttributes = currentStarship.shipAttributes;
            currentPlanetInfo = planetInfo;

            currentShipCamera = mainCamera;
            currentShipCamera.transform.parent = starship.CameraPoint;
            currentShipCamera.transform.localPosition = Vector3.zero;
            currentShipCamera.transform.localRotation = Quaternion.identity;

            Cursor.lockState = CursorLockMode.Locked;
            landingPosition = position;
            landingRotation = rotation;

            currentStarship.transform.DORotate(landingRotation.eulerAngles, 3f);
            currentStarship.transform.DOMove(landingPosition, 4f).OnComplete(FinishLanding);
        }

        public void Stop()
        {
            isActive = false;
        }

        public void UpdateMode()
        {
            // if (!isActive)
            // {
            //     return;
            // }
            //
            // landingProgress += LANDING_SPEED * Time.deltaTime;
            // currentStarship.transform
            //         .DOLocalMove(landingPosition,) = //Vector3.Lerp(startPosition, landingPosition, landingProgress);
            //     currentStarship.transform.localRotation =
            //         Quaternion.Euler(Vector3.Lerp(startRotation, landingRotation, landingProgress));
            // if (landingProgress >= 1)
            // {
            //     FinishLanding();
            // }
        }

        private void FinishLanding()
        {
            Debug.Log("Stop landing");

            Vector3 roverSpawnPoint = FindBestLandPointInDistance(currentPlanetInfo);
            Quaternion roverSpawnRotation = Quaternion.LookRotation(roverSpawnPoint - currentPlanetInfo.Planet.transform.position, Vector3.up) * Quaternion.Euler(90, 0, 0);

            if(roverSpawnPoint == null)
            {
                Debug.Log("Point is null, trying to find another");
                GameObject point = FindClosestLandPointInDistance(currentPlanetInfo, ROVER_SPAWN_DISTANCE);
                roverSpawnPoint = point.transform.position;
                roverSpawnRotation = point.transform.rotation;
            }

            roverSpawnPoint += roverSpawnRotation * Vector3.up * 2f;

            GameManager.Instance.StartRoverMode(currentStarship, currentPlanetInfo, roverSpawnPoint, roverSpawnRotation);

            Stop();
        }

        private Vector3 FindBestLandPointInDistance(WorldInfo.PlanetObjectsInfo planetObjectsInfo)
        {
            Transform sTransform = currentStarship.transform;
            Vector3 topPosition = sTransform.position + sTransform.up * 10f;

            Vector3[] raycastPoints = new Vector3[4];

            raycastPoints[0] = topPosition + sTransform.forward * ROVER_SPAWN_DISTANCE;
            raycastPoints[1] = topPosition + sTransform.right * ROVER_SPAWN_DISTANCE;
            raycastPoints[2] = topPosition - sTransform.forward * ROVER_SPAWN_DISTANCE;
            raycastPoints[3] = topPosition - sTransform.right * ROVER_SPAWN_DISTANCE;

            List<Vector3> spawnPoints = new List<Vector3>();

            foreach(Vector3 raycastPoint in raycastPoints)
            {
                if (Physics.Raycast(raycastPoint, planetObjectsInfo.Planet.transform.position - raycastPoint,
                    out RaycastHit hit, planetObjectsInfo.Planet.elevationMinMax.Max + 100f, 64))
                {
                    spawnPoints.Add(hit.point);
                }
                else
                {
                    Debug.Log("Rover spawn point fail");
                }
            }

            Vector3 point = spawnPoints.OrderBy(r => (r - currentStarship.transform.position).sqrMagnitude).First();

            return point;
        }

        private GameObject FindClosestLandPointInDistance(WorldInfo.PlanetObjectsInfo planetObjectsInfo, float distance)
        {
            var point = planetObjectsInfo.Generator.spawnPoints
                .Where(r => planetObjectsInfo.Generator.takenPoints.All(d =>
                    d.transform.localPosition != r.transform.localPosition))
                .Where(r =>
                    WorldInfo.Instance.placedBuildings.All(b =>
                        Vector3.Distance(b.Value.transform.localPosition, r.transform.localPosition) >
                        WorldInfo.MinDistanceBetweenBuildings))
                .Where(r => Vector3.Distance(r.transform.localPosition, currentStarship.transform.localPosition) >
                distance)
                .OrderBy(r => Vector3.Distance(currentStarship.transform.position, r.transform.position)).First();
            return point;
        }
    }
}