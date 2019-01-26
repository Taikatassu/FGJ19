using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour {

    private EventManager em;
    private Camera mainCamera;
    private bool placementModeState = false;
    private GameObject objectToPlace;
    private List<GameObject> placedObjects;

    public float planetRadius;
    public Transform dynamicsParent;
    [Header("Testing options")]
    public GameObject testPlacementObject;
    public KeyCode testButton = KeyCode.Space;

    private void OnEnable() {
        placedObjects = new List<GameObject>();
        mainCamera = Camera.main;

        em = EventManager._instance;
        em.OnPlacmentModeEnabled += OnPlacementModeEnabled;
        em.OnPlacementModeDisabled += OnPlacementModeDisabled;
    }

    private void OnDisable() {
        em.OnPlacmentModeEnabled -= OnPlacementModeEnabled;
        em.OnPlacementModeDisabled -= OnPlacementModeDisabled;
    }

    private void OnPlacementModeEnabled(GameObject newObjectToPlace) {
        placementModeState = true;
        objectToPlace = Instantiate(newObjectToPlace, dynamicsParent);
    }

    private void OnPlacementModeDisabled() {
        placementModeState = false;

        if(objectToPlace != null) {
            Destroy(objectToPlace);
            objectToPlace = null;
        }
    }

    private void Update() {

        if(Input.GetKeyDown(testButton)) {
            if(!placementModeState) {
                em.BroadcastPlacementModeEnabled(testPlacementObject);
            } else {
                em.BroadcastPlacementModeDisabled();
            }
        }

        if(placementModeState) {
            Vector2 inputPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 snappedPosition = inputPosition.normalized * planetRadius;

            objectToPlace.transform.position = snappedPosition;
            objectToPlace.transform.rotation
                = Quaternion.LookRotation(Vector3.forward, snappedPosition);

            if(Input.GetMouseButtonUp(0)) {
                placedObjects.Add(objectToPlace);
                objectToPlace = null;
                placementModeState = false;
                em.BroadcastPlacementModeDisabled();
            }
        }
    }
}
