using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatingGrabbableSpawner))]
public class FloatingGrabbaleSpawnerEditor : Editor {

    public override void OnInspectorGUI() {

        DrawDefaultInspector();
        FloatingGrabbableSpawner _target = (FloatingGrabbableSpawner)target;

        if(GUILayout.Button("Generate points")) {
            _target.SpawnObjects();
        }
    }
}

public class FloatingGrabbableSpawner : MonoBehaviour {
    public float radius = 0.5f;
    public float minDistanceFromCenter = 2f;
    public float maxDistanceFromCenter = 15.0f;
    public int rejectionSamples = 30;

    public Transform dynamicsParent;
    public GrabbableInfo[] grabbablesToSpawn;

    List<Vector2> points;
    List<GameObject> spawnedGrabbables;
    Transform grabbableParent;
    string grabbableParentName = "Grabbables";

    EventManager em;

    private void Awake() {
        em = EventManager._instance;
    }

    private void OnEnable() {
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
        points = PoissonDiscSamplingService.GeneratePoints(radius, minDistanceFromCenter,
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

            foreach(var point in points) {
                GameObject spawnedObject = Instantiate(grabbablesToSpawn[Random.Range(0,
                    grabbablesToSpawn.Length)].floatingPrefab, point + offset,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), grabbableParent);
                spawnedGrabbables.Add(spawnedObject);
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
