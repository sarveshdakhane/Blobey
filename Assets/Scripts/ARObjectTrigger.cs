using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ARObjectTrigger : MonoBehaviour
{
    public MidAirPositionerBehaviour plane;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(waiter());
        
    }

   
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(4);
        Vector2 aPosition = new Vector2(0, 0);
        plane.ConfirmAnchorPosition(aPosition);
    }
}
