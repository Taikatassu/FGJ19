using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryButton : MonoBehaviour
{
    EventManager em;
    GameObject hook;
    GameObject grabbedObject;

    void Awake()
    {
        print("AWAKE!!");
        em = EventManager._instance;
        em.OnHookCreated += OnHookCreated;
        gameObject.SetActive(false);
    }

    void OnHookCreated(GameObject newHook)
    {
        hook = newHook;
        hook.GetComponent<Hook>().OnHookCollision += OnHookCollision;
    }

    void OnHookCollision(Collider2D col)
    {
        print("moiii");
        gameObject.SetActive(true);
        grabbedObject = col.gameObject;
    }

    public void Deliver()
    {
        em.BroadcastPlacementModeEnabled(grabbedObject);
        Destroy(grabbedObject);
    }
}
