using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour {

    private EventManager em;
    private Camera mainCamera;
    private bool placementModeState = false;
    private GameObject objectToPlace;
    private List<GameObject> placedObjects;
    private bool mouseDownAfterPlacementModeEnabled = false;

    public float minPlanetWalkSpeed = 2f;
    public float maxPlanetWalkSpeed = 12f;
    public float planetRadius;
    public Transform dynamicsParent;
    [Header("Testing options")]
    public GameObject testPlacementObject;
    public KeyCode testButton = KeyCode.Space;

    private void OnEnable() {
        placedObjects = new List<GameObject>();
        mainCamera = Camera.main;

        em = EventManager._instance;
        em.OnPlacementModeEnabled += OnPlacementModeEnabled;
        em.OnPlacementModeDisabled += OnPlacementModeDisabled;
    }

    private void OnDisable() {
        em.OnPlacementModeEnabled -= OnPlacementModeEnabled;
        em.OnPlacementModeDisabled -= OnPlacementModeDisabled;
    }

    private void OnPlacementModeEnabled(GameObject newObjectToPlace) {
        if (newObjectToPlace == null) {
            Debug.LogWarning("Prevented placement mode enabling with null object reference!");
            return;
        }

        placementModeState = true;
        mouseDownAfterPlacementModeEnabled = false;
        objectToPlace = Instantiate(newObjectToPlace, dynamicsParent);
        objectToPlace.GetComponent<RandomRotator>().enabled = false;
    }

    private void OnPlacementModeDisabled() {
        placementModeState = false;

        if (objectToPlace != null) {
            Destroy(objectToPlace);
            objectToPlace = null;
        }
    }

    IEnumerator WaitToDisablePlacement() {
        yield return new WaitForEndOfFrame();
        em.BroadcastPlacementModeDisabled();
    }

    private void Update() {
        if (Input.GetKeyDown(testButton)) {
            if (!placementModeState) {
                em.BroadcastPlacementModeEnabled(testPlacementObject);
            } else {
                em.BroadcastPlacementModeDisabled();
            }
        }

        if (placementModeState) {
            Vector2 inputPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 snappedPosition = inputPosition.normalized * planetRadius;

            objectToPlace.transform.position = snappedPosition;
            objectToPlace.transform.rotation
                = Quaternion.LookRotation(Vector3.forward, snappedPosition);

            if (Input.GetMouseButtonDown(0)) {
                mouseDownAfterPlacementModeEnabled = true;
            }

            if (Input.GetMouseButtonUp(0) && mouseDownAfterPlacementModeEnabled) {
                objectToPlace.GetComponent<Collider2D>().enabled = false;
                Animator animator = objectToPlace.GetComponentInChildren<Animator>();
                if (animator != null) {
                    animator.Play("Spawn");
                }

                if (objectToPlace.CompareTag("Walker")) {
                    PlanetWalkController walkScript = objectToPlace.AddComponent<PlanetWalkController>();
                    walkScript.InitializePlanetWalk(minPlanetWalkSpeed, maxPlanetWalkSpeed, planetRadius);
                }

                placedObjects.Add(objectToPlace);
                objectToPlace = null;
                placementModeState = false;
                em.BroadcastPlacedObjectCountChanged(placedObjects.Count);
                StartCoroutine("WaitToDisablePlacement");
            }
        }
    }
}
