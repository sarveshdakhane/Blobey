namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
	using Mapbox.Unity.Map;
	using UnityEngine;
	using GetTheLocation;
	using Mapbox.Unity.Utilities;
	using UnityEngine.UI;
    using System.Collections;

    public class ImmediatePositionWithLocationProvider : MonoBehaviour
	{

        public GameObject _GetLocation;
		bool _isInitialized;

		ILocationProvider _locationProvider;
		ILocationProvider LocationProvider
		{
			get
			{
				if (_locationProvider == null)
				{
					_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
		}


		void Start()
		{
			LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
		}

        void LateUpdate()
        {
            if (_isInitialized)
            {
                var map = LocationProviderFactory.Instance.mapManager;               
                ////transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);       
                transform.localPosition = map.GeoToWorldPosition(VectorExtensions.ToVector2d(_GetLocation.GetComponent<GettheLocation>().CurrentLocationVector2D));
            }
        }	   

    }
}