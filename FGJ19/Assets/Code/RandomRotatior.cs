using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotatior : MonoBehaviour {
    public float minRotationSpeed = 5f;
    public float maxRotationSpeed = 25f;

    private float rotationSpeed = 0f;
    private Transform t;

    private void Start() {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        if(Random.value < 0.5f) rotationSpeed *= -1f;
        t = transform;
    }

    private void Update() {
        t.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
