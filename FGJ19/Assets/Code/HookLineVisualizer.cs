using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLineVisualizer : MonoBehaviour {
    EventManager em;
    public Transform startPoint;
    public Transform endPoint;

    private LineRenderer lineRenderer;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable() {
        em = EventManager._instance;
        em.OnHookCreated += OnHookCreated;
    }

    private void OnDisable() {
        em.OnHookCreated -= OnHookCreated;
    }

    private void OnHookCreated(GameObject newHook) {
        endPoint = newHook.transform;
    }

    private void LateUpdate() {
        if(startPoint != null && endPoint != null) {
            Vector3[] linePositions = new Vector3[2] {
                startPoint.position,
                endPoint.position
            };
            lineRenderer.SetPositions(linePositions);
        }
    }
}
