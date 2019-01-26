using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {
    public Action<Collider2D> OnHookCollision;

    void OnCollisionEnter2D(Collision2D other) {
        print("Hook.OnCollisionEnter2D");
        if(other.collider.gameObject.tag != "Moon") {
            OnHookCollision?.Invoke(other.collider);
        }
    }
}
