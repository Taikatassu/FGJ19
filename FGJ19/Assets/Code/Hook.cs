using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {
    public Action<Collider2D> OnHookCollision;
    public Transform player;
    private Transform t;

    private void Start() {
        t = transform;
    }

    private void Update() {
        //Keep rotation fixed relative to player
        t.rotation = Quaternion.LookRotation(Vector3.forward, player.position - t.position);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.gameObject.tag != "Moon") {
            OnHookCollision?.Invoke(other.collider);
        }
    }
}
