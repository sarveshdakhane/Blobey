using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mapbox.Unity.Location;
using System;
using System.Globalization;
using UnityEngine.UI;
using GetTheLocation;

public class GPSCore : MonoBehaviour
{
    private float _Tlat;
    private float _Tlon;
    private double distance;


    public float Radius = 5f; // Range of Target function start
    private string lat;
    private string lon;


    //private string[] Temp_lat = new string[] { "50.900477538065594", "50.900571617091934" , "50.90061957887535" , "50.900682298056026" , "50.90063618101963" };
    //private string[] Temp_lon = new string[] { "8.047347718318687", "8.046987947091218","8.046657425557038", "8.046362003654808", "8.046183580525739" };

    public GameObject _locationNotifaction; // The list of objects for every points that we want to show
    public GameObject ToInputField;         //Get the target location Coordinate values
    public GameObject _GetLocation;


    public bool TargetPupUpOneTime = false; // for check the popup appear one time not everytime when we are in the range of target
    public bool NoGPSPupUpOneTime = false; // for check the popup appear one time not everytime


    public UnityEvent EventStartGPS; // This event will work when the GPS system start to work
    public UnityEvent EventReachGPSPoint; // This event will work when player reached the GPS point
    public UnityEvent EventOutGPSPointRange; // This event will work when player is out of the GPS point range




    public void StartRadar()
    {

       // Debug.Log("Radar Started");
        _Tlat = float.Parse(ToInputField.GetComponent<InputField>().text.ToString().Split(',')[0].Trim(), CultureInfo.InvariantCulture.NumberFormat);
        _Tlon = float.Parse(ToInputField.GetComponent<InputField>().text.ToString().Split(',')[1].Trim(), CultureInfo.InvariantCulture.NumberFormat);
        StartCoroutine("RadiusScanner");
        if (EventStartGPS != null)
        {
            EventStartGPS.Invoke();
        }
    }


    public IEnumerator RadiusScanner()
    {
        while (true)
        {


            yield return new WaitForSeconds(2);
            lat = _GetLocation.GetComponent<GettheLocation>().CurrentLocationVector2D.x.ToString();
            lon = _GetLocation.GetComponent<GettheLocation>().CurrentLocationVector2D.z.ToString();
            //lat = Temp_lat[i];
            //lon = Temp_lon[i];
            //Debug.Log("Current Values are" + lat + "," + lon);
            Calc(_Tlat, _Tlon, float.Parse(lat, CultureInfo.InvariantCulture.NumberFormat), float.Parse(lon, CultureInfo.InvariantCulture.NumberFormat));
            
     

        }
    }

    public void Calc(float lat1, float lon1, float lat2, float lon2)
    {
        var R = 6378.137; // Radius of earth in KM
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
            Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
            Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        distance = R * c;
        distance = distance * 1000f;


        if (distance < Radius)
        {


            if (TargetPupUpOneTime == false)
            {
                TargetPupUpOneTime = true;

                //for (int i = 0; i < TargetPopUp.Length; i++)
                //{
                //    TargetPopUp[i].SetActive(false);
                //    PointObjects[i].SetActive(false);
                //}

                /*
                TargetPopUp[PointCounter].SetActive(true);
                PointObjects[PointCounter].SetActive(true);
                */

                _locationNotifaction.SetActive(true);


                if (EventReachGPSPoint != null)
                {
                    EventReachGPSPoint.Invoke();
                }

            }
        }
        if (distance > Radius)
        {
            
            /*
            for (int i = 0; i < TargetPopUp.Length; i++)
            {
                TargetPopUp[i].SetActive(false);
                PointObjects[i].SetActive(false);
            }
            PointCounter++;
            if (PointCounter == Lat.Length)
            {
                PointCounter = 0;
            }
            */

            // ReachedObject.SetActive(false);
            //  _locationNotifaction.SetActive(true);



            if (EventOutGPSPointRange != null)
            {
                EventOutGPSPointRange.Invoke();
            }
        }
    }


    
    public void HideGameObjects()
    {
         GameObject temp  =   GameObject.Find("DirectionGameObjects");

        if (temp.transform.childCount > 0)
        {
            foreach (Transform child in temp.transform)
            {
                child.gameObject.SetActive(false);

            }
        }

        //GameObject.Find("direction waypoint entity").SetActive(false);
        //TargetPopUp[PointCounter].SetActive(false);
        //PointObjects[PointCounter].SetActive(false);
        //TargetPupUpOneTime = true;
    }
  

    
    public void ShowGameObjects()
    {
        GameObject temp = GameObject.Find("DirectionGameObjects");

        if (temp.transform.childCount > 0)
        {
            foreach (Transform child in temp.transform)
            {
                child.gameObject.SetActive(true);

            }
        }

       // GameObject.Find("direction waypoint entity").SetActive(true);
    }
    
}