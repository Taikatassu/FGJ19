using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    public Action<Collider2D> OnHookCollision;

    void OnCollisionEnter2D(Collision2D other)
    {
        print("collision happened");
        OnHookCollision?.Invoke(other.collider);
        //send info to player script that an object has been hit
        //stop player's hooklerp
        //send other.transform.position to player
        //start lerping player to the other.transform.position
    }
}
