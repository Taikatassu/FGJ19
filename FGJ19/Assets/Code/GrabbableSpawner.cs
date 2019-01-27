using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableSpawner : MonoBehaviour {

    public float objectRadius = 0.5f;
    public float minDistanceFromCenter = 2f;
    public float maxDistanceFromCenter = 15.0f;
    public int rejectionSamples = 30;

    public Transform dynamicsParent;
    public GrabbableInfo[] grabbablesToSpawn;
    public AnimationCurve spawnDensityFalloffTowardsRegionEdges;

    List<Vector2> points;
    List<GameObject> spawnedGrabbables;
    Transform grabbableParent;
    string grabbableParentName = "Grabbables";

    EventManager em;

    private void OnEnable() {
        em = EventManager._instance;
        em.OnStartGame += OnStartGame;
    }

    private void OnDisable() {
        em.OnStartGame -= OnStartGame;
    }

    private void OnStartGame() {
        SpawnObjects();
    }

    public void SpawnObjects() {
        CalculatePoints();
        ClearObjects();
        PlaceObjects();
    }

    private void CalculatePoints() {
        points = PoissonDiscSamplingService.GeneratePoints(objectRadius, minDistanceFromCenter,
            maxDistanceFromCenter, rejectionSamples);
    }

    private void GetOrCreateGrabbableParent() {
        if(grabbableParent == null) {

            grabbableParent = GameObject.Find(grabbableParentName)?.transform;

            if(grabbableParent == null) {
                grabbableParent = new GameObject(grabbableParentName).transform;
                grabbableParent.SetParent(dynamicsParent);
            }
        }
    }

    private void PlaceObjects() {
        if(points != null && grabbablesToSpawn.Length > 0) {
            GetOrCreateGrabbableParent();
            spawnedGrabbables = new List<GameObject>();
            float regionRadius = maxDistanceFromCenter * 2 + 1f;
            float halfRegionRadius = regionRadius / 2;
            Vector2 offset = -Vector2.one * halfRegionRadius;
            float spawnAreaRadius = maxDistanceFromCenter - minDistanceFromCenter;

            //Create a list of rarity thresholds
            float[] grabbableRarityThresholds = new float[grabbablesToSpawn.Length];
            grabbableRarityThresholds[0] = (grabbablesToSpawn[0].rarity);
            for(int i = 1; i < grabbablesToSpawn.Length; i++) {
                grabbableRarityThresholds[i] = (grabbableRarityThresholds[i - 1]
                    + grabbablesToSpawn[i].rarity);
            }
            float maxRarityThreshold
                = grabbableRarityThresholds[grabbableRarityThresholds.Length - 1];

            foreach(var point in points) {
                //Calculate placement for a grabbable on the planet surface
                Vector2 spawnPosition = point + offset;
                float positionDstFromMinEdge = spawnPosition.magnitude - minDistanceFromCenter;
                float positionDstPercentage = positionDstFromMinEdge / spawnAreaRadius;
                if(Random.value > spawnDensityFalloffTowardsRegionEdges.
                    Evaluate(positionDstPercentage)) {
                    continue;
                }

                //Choose random grabbable to spawn taking account the rarity weights
                float spawnRandom = Random.value * maxRarityThreshold;
                GrabbableInfo grabbableToSpawn = null;
                for(int i = 0; i < grabbableRarityThresholds.Length; i++) {
                    if(spawnRandom < grabbableRarityThresholds[i]) {
                        grabbableToSpawn = grabbablesToSpawn[i];
                        break;
                    }
                }

                //Instantiate the chosen grabbable
                GameObject spawnedObject = Instantiate(grabbableToSpawn.grabbablePrefab,
                    spawnPosition, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    grabbableParent);
                spawnedGrabbables.Add(spawnedObject);

                //GameObject spawnedObject = Instantiate(grabbablesToSpawn[Random.Range(0,
                //    grabbablesToSpawn.Length)].grabbablePrefab, spawnPosition,
                //    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), grabbableParent);
                //spawnedGrabbables.Add(spawnedObject);
            }
        }
    }

    private void ClearObjects() {
        GetOrCreateGrabbableParent();

        if(grabbableParent != null) {
            if(Application.isEditor) {
                DestroyImmediate(grabbableParent.gameObject);
            } else {
                Destroy(grabbableParent.gameObject);
            }
        }

        if(spawnedGrabbables != null) {
            spawnedGrabbables.Clear();
        }
    }
}
