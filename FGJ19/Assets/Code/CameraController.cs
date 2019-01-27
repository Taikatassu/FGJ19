using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private EventManager em;
    private bool followPlayer;
    private Transform t;
    private Camera cam;
    private Vector2 movementRefVel = Vector2.zero;
    private float zoomRefVel = 0f;
    private Vector2 cameraTargetPosition = Vector2.zero;
    private float currentOrthSize;

    public Transform player;
    public float cameraMovementSmoothTime;
    public float cameraZoomSmoothTime;
    public float playerModeOrthSize = 15f;
    public float placementModeOrthSize = 10f;
    public float introModeInitialOrthSize = 30f;

    private void OnEnable() {
        t = transform;
        cam = GetComponent<Camera>();
        em = EventManager._instance;

        em.OnStartGame += OnStartGame;
        em.OnPlacementModeEnabled += EnablePlanetView;
        em.OnPlacementModeDisabled += EnablePlayerView;
        em.OnGameOver += EnablePlanetView;
        em.OnKeepGoing += EnablePlayerView;
    }

    private void OnDisable() {
        em.OnStartGame -= OnStartGame;
        em.OnPlacementModeEnabled -= EnablePlanetView;
        em.OnPlacementModeDisabled -= EnablePlayerView;
        em.OnGameOver -= EnablePlanetView;
        em.OnKeepGoing -= EnablePlayerView;
    }

    private void OnStartGame() {
        t.position = player.transform.position;
        cam.orthographicSize = introModeInitialOrthSize;
    }
    
    private void Update() {
        if (followPlayer) {
            cameraTargetPosition = player.position;
        }

        Vector2 newCameraPosition = Vector2.SmoothDamp(t.position, cameraTargetPosition,
            ref movementRefVel, cameraMovementSmoothTime);
        t.position = new Vector3(newCameraPosition.x, newCameraPosition.y, -10f);

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, currentOrthSize,
            ref zoomRefVel, cameraZoomSmoothTime);
    }

    private void EnablePlanetView() {
        followPlayer = false;
        cameraTargetPosition = Vector2.zero;
        currentOrthSize = placementModeOrthSize;
    }

    //Note: GameObject parameter enables subscription to placement mode event
    private void EnablePlanetView(GameObject obj) {
        EnablePlanetView();
    }

    private void EnablePlayerView() {
        followPlayer = true;
        currentOrthSize = playerModeOrthSize;
    }
}
