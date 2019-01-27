using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GrabbableSpawner))]
public class FloatingGrabbaleSpawnerEditor : Editor {

    public override void OnInspectorGUI() {

        DrawDefaultInspector();
        GrabbableSpawner _target = (GrabbableSpawner)target;

        if (GUILayout.Button("Generate points")) {
            _target.SpawnObjects();
        }
    }
}
