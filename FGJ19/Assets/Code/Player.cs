using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Transform targetDirection;
    Transform travelPos;
    public GameObject looker;
    public GameObject rotator;
    public GameObject reticle;
    bool raycastSuccessful = false;
    bool hookLerpOK = false;
    bool hookLerpReturn = false;
    bool playerLerpOK = false;
    public GameObject spawnableHook;
    GameObject hook;
    float currentHookLerpTime;
    public float hookLerpTime;
    float currentPlayerLerpTime;
    public float playerLerpTime;
    Vector3 playerLerpStartPoint;
    Vector3 playerLerpTarget;

    void Start()
    {
        targetDirection = new GameObject("targetDirection").transform;
        travelPos = new GameObject("travelPos").transform;
        hook = Instantiate(spawnableHook, transform.position, transform.rotation);
        hook.GetComponent<Hook>().OnHookCollision += OnHookCollision;
    }

    void OnHookCollision(Collider2D col)
    {
        print("paskaa");
        if (hookLerpOK)
        {
            hookLerpOK = false;
            hookLerpReturn = false;
            playerLerpStartPoint = transform.position;
            playerLerpTarget = hook.transform.position;
            currentPlayerLerpTime = 0;
            playerLerpOK = true;
        }
    }


    void Update()
    {
        if (!hookLerpOK && !hookLerpReturn && !playerLerpOK)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit2D hitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
                Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction, Color.red, 5f);
                if (hitInfo.collider != null)
                {
                    raycastSuccessful = true;
                    targetDirection.position = hitInfo.point;

                    //Make looker look at input position
                    //looker.transform.LookAt(targetDirection);
                    var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(looker.transform.position);
                    var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    looker.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    //Enable looker
                    looker.SetActive(true);
                }
                else
                {
                    raycastSuccessful = false;
                }
            }
            if (Input.GetMouseButtonUp(0) && raycastSuccessful)
            {
                looker.SetActive(false);
                travelPos.position = reticle.transform.position;
                currentHookLerpTime = 0;
                hookLerpOK = true;
            }
        }

        if (hookLerpOK)
        {
            //increment timer once per frame
            currentHookLerpTime += Time.deltaTime;
            if (currentHookLerpTime > hookLerpTime)
            {
                currentHookLerpTime = hookLerpTime;
            }

            //lerp!
            float perc = currentHookLerpTime / hookLerpTime;
            hook.transform.position = Vector3.Lerp(transform.position, travelPos.position, perc);
            if (currentHookLerpTime == hookLerpTime)
            {
                hookLerpOK = false;
                hookLerpReturn = true;
                currentHookLerpTime = 0;
            }
        }
        if (hookLerpReturn)
        {
            //increment timer once per frame
            currentHookLerpTime += Time.deltaTime;
            if (currentHookLerpTime > hookLerpTime)
            {
                currentHookLerpTime = hookLerpTime;
            }

            //lerp!
            float perc = currentHookLerpTime / hookLerpTime;
            hook.transform.position = Vector3.Lerp(travelPos.position, transform.position, perc);
            if (currentHookLerpTime == hookLerpTime)
            {
                hookLerpReturn = false;
            }
        }
        if (playerLerpOK)
        {
            //increment timer once per frame
            currentPlayerLerpTime += Time.deltaTime;
            if (currentPlayerLerpTime > playerLerpTime)
            {
                currentPlayerLerpTime = playerLerpTime;
            }

            //lerp!
            float perc = currentPlayerLerpTime / playerLerpTime;
            transform.position = Vector3.Lerp(playerLerpStartPoint, playerLerpTarget, perc);
            print(currentPlayerLerpTime);
            if (currentPlayerLerpTime == playerLerpTime)
            {
                playerLerpOK = false;
                print("lerp finished");
            }
        }
    }
}
