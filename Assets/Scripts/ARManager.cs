using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;
public class ARManager : MonoBehaviour
{



  

    

    //private SmartTerrain smartTerrain;
    //PositionalDeviceTracker positionalDeviceTracker;

    ContentPositioningBehaviour contentPositioningBehaviour;
    AnchorBehaviour planeAnchor, midAirAnchor, placementAnchor;
    //StateManager stateManager;
    float touchesPrePosDifference, touchesCurPosDifference, zoomModifier;
    Vector2 Test, firstTouchPrevPos, secondTouchPrevPos;
    public GameObject obj; // The 3d model that we want to show on the position
    public bool ShowModelState = false;
    public bool CanSupport = false;
    private string consoleString; // Current value of message in console




    public void LaunchModel()
    {
        ShowModel();
    }
    public void ShowModel()
    {
        obj.SetActive(true);
        ShowModelState = true;
    }
    public void HideModel()
    {
        obj.SetActive(false);
        ShowModelState = false;
    }


    //public void ResetTrackers()
    //{
    //    this.smartTerrain = TrackerManager.Instance.GetTracker();
    //    this.positionalDeviceTracker = TrackerManager.Instance.GetTracker();
    //    this.smartTerrain.Stop();
    //    this.positionalDeviceTracker.Reset();
    //    this.smartTerrain.Start();
    //}

}
