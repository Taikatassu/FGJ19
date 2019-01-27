using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetWalkController : MonoBehaviour {

    private bool planetWalking = false;
    private Transform t;
    private float currentWalkSpeed;
    private float planetRadius;

    public void InitializePlanetWalk(float minWalkSpeed, float maxWalkSpeed, float planetRadius) {
        planetWalking = true;
        t = transform;
        this.planetRadius = planetRadius;
        currentWalkSpeed = Random.Range(minWalkSpeed, maxWalkSpeed);
    }

    void Update() {
        if (planetWalking) {
            Vector3 newPosition = t.position + t.right * -currentWalkSpeed * Time.deltaTime;
            newPosition = newPosition.normalized * planetRadius;
            t.position = newPosition;
            t.rotation = Quaternion.LookRotation(Vector3.forward, newPosition);
        }
    }
}
