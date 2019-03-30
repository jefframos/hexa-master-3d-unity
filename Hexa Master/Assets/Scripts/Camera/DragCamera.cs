using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCamera : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private Vector3 targetPos = new Vector3();

    public bool cameraDragging = true;
    public bool cameraHolding = false;

    public float outerLeft = -10f;
    public float outerRight = 10f;

    public float outerTop = -3f;
    public float outerDown = 3f;

    public Transform target;


    void LateUpdate()
    {



        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        float left = Screen.width * 0.2f;
        float right = Screen.width - (Screen.width * 0.2f);

        if (mousePosition.x < left)
        {
            cameraDragging = true;
        }
        else if (mousePosition.x > right)
        {
            cameraDragging = true;
        }



        //Debug.Log(Input.GetMouseButtonDown(0));
        if (Input.GetMouseButton(0))
        {
            if (!cameraHolding)
            {
                dragOrigin = Input.mousePosition;

            }
            cameraHolding = true;

            
        }
        else
        {
            cameraHolding = false;
        }

        if (cameraDragging && cameraHolding)
        {
            
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

            targetPos =  Vector3.Lerp(move,targetPos,0.5f);

            if(targetPos.x < 0 && target.transform.position.x < outerLeft || targetPos.x > 0 &&  target.transform.position.x > outerRight)
            {
                targetPos.x = 0;

            }

            if (targetPos.y < 0 && target.transform.position.y < outerTop || targetPos.y > 0 && target.transform.position.y > outerDown)
            {
                targetPos.y = 0;

            }
        }
        else
        {
            targetPos = Vector3.Lerp(Vector3.zero, targetPos, 0.8f);
        }
        //Debug.Log(targetPos);
        target.Translate(targetPos, Space.World);
    }


}