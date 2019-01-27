using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryButton : MonoBehaviour {
    EventManager em;
    GameObject hook;
    GameObject grabbedObject;

    void Awake() {
        em = EventManager._instance;
        em.OnHookCreated += OnHookCreated;
        gameObject.SetActive(false);
    }

    void OnHookCreated(GameObject newHook) {
        hook = newHook;
        hook.GetComponent<Hook>().OnHookCollision += OnHookCollision;
    }

    void OnHookCollision(Collider2D col) {
        gameObject.SetActive(true);
        grabbedObject = col.gameObject;
    }

    public void Deliver() {
        em.BroadcastPlacementModeEnabled(grabbedObject);
        Destroy(grabbedObject);
        gameObject.SetActive(false);
    }
}
