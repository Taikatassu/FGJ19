using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationAngle = 90;
    float trueRotAngle;
    float currentLerpTime;
    public float lerpTime = 1;
    bool rotateRight = false;
    bool rotateLeft = false;
    void Start()
    {
        trueRotAngle = rotationAngle / 2;
        transform.localEulerAngles = new Vector3(0, 0, trueRotAngle);
        rotateRight = true;
    }


    void Update()
    {
        if (rotateRight)
        {
            //increment timer once per frame
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp!
            float perc = currentLerpTime / lerpTime;
            float angle = Mathf.LerpAngle(trueRotAngle, -trueRotAngle, perc);
            transform.localEulerAngles = new Vector3(0, 0, angle);
            if (currentLerpTime == lerpTime)
            {
                rotateRight = false;
                rotateLeft = true;
                currentLerpTime = 0;
            }
        }
        if (rotateLeft)
        {
            //increment timer once per frame
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp!
            float perc = currentLerpTime / lerpTime;
            float angle = Mathf.LerpAngle(-trueRotAngle, trueRotAngle, perc);
            transform.localEulerAngles = new Vector3(0, 0, angle);
            if (currentLerpTime == lerpTime)
            {
                rotateLeft = false;
                rotateRight = true;
                currentLerpTime = 0;
            }
        }
    }
}
