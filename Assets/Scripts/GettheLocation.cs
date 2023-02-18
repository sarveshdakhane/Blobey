using System.Collections;
using System.Globalization;
using UnityEngine;

namespace GetTheLocation
{
    public class GettheLocation : MonoBehaviour
    {

        private static string Lat = "50.900339186211156";
        private static string Lon = "8.04740036780769";

        private Vector3 _CurrentLocationVector2D;

        public Vector3 CurrentLocationVector2D
        {
            get
            {
                return _CurrentLocationVector2D;
            }
        }

        private string _CurrentLocation;

        public string CurrentLocation
        {
            get
            {
                return _CurrentLocation;
            }
        }

        private void Start()
        {
            // Call the GPS connection in native and try to connect to the satelite
            Input.location.Start();
            StartCoroutine("GPSProcess");
        }


        public IEnumerator GPSProcess()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                if (Input.location.isEnabledByUser == true)
                {
                    Input.location.Start();
                    _CurrentLocationVector2D.x = Input.location.lastData.latitude;
                    _CurrentLocationVector2D.z = Input.location.lastData.longitude;
                    _CurrentLocation = Input.location.lastData.latitude + "," + Input.location.lastData.longitude;
                }
                else
                {


                    _CurrentLocationVector2D.x = float.Parse(Lat, CultureInfo.InvariantCulture.NumberFormat);
                    _CurrentLocationVector2D.z = float.Parse(Lon, CultureInfo.InvariantCulture.NumberFormat);
                    _CurrentLocation = Lat + "," + Lon;
                }
            }
        }


    }
}