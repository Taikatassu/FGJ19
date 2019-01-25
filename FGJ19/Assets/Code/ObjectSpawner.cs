using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor {

    public override void OnInspectorGUI() {

        DrawDefaultInspector();
        ObjectSpawner _target = (ObjectSpawner)target;

        if(GUILayout.Button("Generate points")) {
            _target.DrawPoints();
        }
    }
}

public class ObjectSpawner : MonoBehaviour {
    public float radius = 0.5f;
    public float minDistanceFromCenter = 2f;
    public float maxDistanceFromCenter = 15.0f;
    public int rejectionSamples = 30;
    public float displayRadius = 0.5f;

    List<Vector2> points;

    public void DrawPoints() {
        points = PoissonDiscSampling.GeneratePoints(radius, minDistanceFromCenter,
            maxDistanceFromCenter, rejectionSamples);
    }

    private void OnDrawGizmos() {
        float regionRadius = maxDistanceFromCenter * 2 + 1f;

        Gizmos.DrawWireCube(Vector2.one * regionRadius / 2, Vector2.one * regionRadius);

        if(points != null) {
            foreach(var point in points) {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }
}
