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
    Transform hook;
    float currentHookLerpTime;
    public float hookLerpTime;

    void Start()
    {
        targetDirection = new GameObject("targetDirection").transform;
        travelPos = new GameObject("travelPos").transform;
        hook = new GameObject("hook").transform;
    }


    void Update()
    {
        if (!hookLerpOK && !hookLerpReturn)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit2D hitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
                Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction, Color.red, 5f);
                print("raycasted");
                if (hitInfo.collider != null)
                {
                    raycastSuccessful = true;
                    targetDirection.position = hitInfo.point;
                    print("raycast position: " + hitInfo.point);

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
                    print("raycast failed!");
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
            hook.position = Vector3.Lerp(transform.position, travelPos.position, perc);
            print("currenthooklerptime = " + currentHookLerpTime);
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
            hook.position = Vector3.Lerp(travelPos.position, transform.position, perc);
            print("currenthooklerptime = " + currentHookLerpTime);
            if (currentHookLerpTime == hookLerpTime)
            {
                hookLerpReturn = false;
            }
        }
    }
}
