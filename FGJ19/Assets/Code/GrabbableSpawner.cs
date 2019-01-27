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

    List<Vector2> points = new List<Vector2>();
    List<GameObject> spawnedGrabbables = new List<GameObject>();

    EventManager em;

    private void Awake() {
        BruteforceClearGrabbables();
    }

    private void OnEnable() {
        em = EventManager._instance;
        em.OnStartGame += OnStartGame;
    }

    private void OnDisable() {
        em.OnStartGame -= OnStartGame;
    }

    private void OnStartGame() {
        SpawnGrabbables();
    }

    public void SpawnGrabbables() {
        CalculatePoints();
        ClearGrabbables();
        PlaceGrabbables();
    }

    private void CalculatePoints() {
        points = PoissonDiscSamplingService.GeneratePoints(objectRadius, minDistanceFromCenter,
            maxDistanceFromCenter, rejectionSamples);
    }

    private void PlaceGrabbables() {
        if (points != null && grabbablesToSpawn.Length > 0) {
            float regionRadius = maxDistanceFromCenter * 2 + 1f;
            float halfRegionRadius = regionRadius / 2;
            Vector2 offset = -Vector2.one * halfRegionRadius;
            float spawnAreaRadius = maxDistanceFromCenter - minDistanceFromCenter;

            //Create a list of rarity thresholds
            float[] grabbableRarityThresholds = new float[grabbablesToSpawn.Length];
            grabbableRarityThresholds[0] = (grabbablesToSpawn[0].rarity);
            for (int i = 1; i < grabbablesToSpawn.Length; i++) {
                grabbableRarityThresholds[i] = (grabbableRarityThresholds[i - 1]
                    + grabbablesToSpawn[i].rarity);
            }
            float maxRarityThreshold
                = grabbableRarityThresholds[grabbableRarityThresholds.Length - 1];

            foreach (var point in points) {
                //Calculate placement for a grabbable on the planet surface
                Vector2 spawnPosition = point + offset;
                float positionDstFromMinEdge = spawnPosition.magnitude - minDistanceFromCenter;
                float positionDstPercentage = positionDstFromMinEdge / spawnAreaRadius;
                if (Random.value > spawnDensityFalloffTowardsRegionEdges.
                    Evaluate(positionDstPercentage)) {
                    continue;
                }

                //Choose random grabbable to spawn taking account the rarity weights
                float spawnRandom = Random.value * maxRarityThreshold;
                GrabbableInfo grabbableToSpawn = null;
                for (int i = 0; i < grabbableRarityThresholds.Length; i++) {
                    if (spawnRandom < grabbableRarityThresholds[i]) {
                        grabbableToSpawn = grabbablesToSpawn[i];
                        break;
                    }
                }

                //Instantiate the chosen grabbable
                GameObject spawnedObject = Instantiate(grabbableToSpawn.grabbablePrefab,
                    spawnPosition, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    dynamicsParent);
                spawnedGrabbables.Add(spawnedObject);
            }
        }
    }

    private void ClearGrabbables() {

        if (spawnedGrabbables.Count > 0) {
            foreach (var grabbable in spawnedGrabbables) {
                if (Application.isPlaying) {
                    Destroy(grabbable);
                } else {
                    DestroyImmediate(grabbable);
                }
            }
        }
    }

    private void BruteforceClearGrabbables() {

        if (dynamicsParent != null) {
            foreach (Transform child in dynamicsParent.GetComponentInChildren<Transform>()) {
                if (child != dynamicsParent) {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
