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
    bool placementEnabled = false;
    bool gameOver = false;
    public bool controlsEnabled = false;
    public GameObject spawnableHook;
    GameObject hook;
    public GameObject hookVFX;
    float currentHookLerpTime;
    public float hookLerpTime;
    float currentPlayerLerpTime;
    public float playerLerpTime;
    Vector3 playerLerpStartPoint;
    Vector3 playerLerpTarget;

    EventManager em;

    void Start()
    {
        targetDirection = new GameObject("targetDirection").transform;
        travelPos = new GameObject("travelPos").transform;
        hook = Instantiate(spawnableHook, transform.position, transform.rotation);
        Hook hookScript = hook.GetComponent<Hook>();
        hookScript.OnHookCollision += OnHookCollision;
        hookScript.player = transform;
        em.BroadcastHookCreated(hook);
    }

    void OnEnable()
    {
        em = EventManager._instance;
        em.OnPlacementModeEnabled += OnPlacementModeEnabled;
        em.OnPlacementModeDisabled += OnPlacementModeDisabled;
        em.OnGameOver += OnGameOver;
        em.OnKeepGoing += OnKeepGoing;
    }

    void OnDisable()
    {
        em.OnPlacementModeEnabled -= OnPlacementModeEnabled;
        em.OnPlacementModeDisabled -= OnPlacementModeDisabled;
        em.OnGameOver -= OnGameOver;
        em.OnKeepGoing -= OnKeepGoing;
    }

    void OnPlacementModeEnabled(GameObject objectToPlace)
    {
        placementEnabled = true;
    }

    void OnPlacementModeDisabled()
    {
        placementEnabled = false;
    }

    private void OnGameOver()
    {
        gameOver = true;
    }

    private void OnKeepGoing()
    {
        gameOver = false;
    }

    void OnHookCollision(Collider2D col)
    {
        if (hookLerpOK)
        {
            hookLerpOK = false;
            hookLerpReturn = false;
            playerLerpStartPoint = transform.position;
            playerLerpTarget = hook.transform.position;
            Instantiate(hookVFX, new Vector3(hook.transform.position.x, hook.transform.position.y, -1), Quaternion.identity);
            currentPlayerLerpTime = 0;
            playerLerpOK = true;
        }
    }

    void Update()
    {
        if (!hookLerpOK && !hookLerpReturn && !playerLerpOK && !placementEnabled && !gameOver && controlsEnabled)
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
            if (currentPlayerLerpTime == playerLerpTime)
            {
                playerLerpOK = false;
            }
        }
    }
}
