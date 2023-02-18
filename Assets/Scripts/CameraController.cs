using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    private bool tap, swipeleft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;

    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool  SwipeLeft { get { return swipeleft; } }
    public bool  SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown{ get { return swipeDown; } }

    public Vector2 StartTouch { get { return startTouch; } set {  startTouch =value; } }


    public Transform Camera;
    private Vector2 CameradesiredPosition;
    [SerializeField] private Vector2 Sensitivity;





    private void Reset()
    {
        StartTouch = swipeDelta= Vector2.zero;
        isDraging = false;

    }

  

    void Update()
    {

        tap = swipeleft = swipeRight = swipeUp = swipeDown = false;

        #region Standalone Inputs

        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            StartTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            Reset();
        }

        #endregion

        #region Mobile Inputs

        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isDraging = true;
                tap = true;
                StartTouch = Input.touches[0].position;

            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();

            }

        }

        #endregion

        // Calcualte the distance 

        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - StartTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - StartTouch;
            }
        }

        //Did we cross the deadzone ?

       if (swipeDelta.magnitude > 100)
       {
            //Which Direction
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //lef or right
                if (x > 0)
                {
                    swipeleft = true;
                }
                else
                {
                    swipeRight = true;
                }
            }
            else
            {
                //up or down
                if (y > 0)
                {
                    swipeDown = true;

                }
                else
                {
                    swipeUp = true;

                }

            }
            Reset();
        }




        if (SwipeLeft)
        {
            CameradesiredPosition += Vector2.left;
        }
        if (SwipeRight)
        {

            CameradesiredPosition += Vector2.right;
        }
        if (SwipeUp)
        {

            CameradesiredPosition += Vector2.up;
        }
        if (SwipeDown)
        {

            CameradesiredPosition += Vector2.down;
        }

        Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, new Vector3(CameradesiredPosition.x * Sensitivity.x, 115, CameradesiredPosition.y * Sensitivity.y), 50f * Time.deltaTime);





    }
    public Vector2 GetInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        return input;
    }

}
