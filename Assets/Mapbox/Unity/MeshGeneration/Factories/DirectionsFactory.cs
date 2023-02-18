namespace Mapbox.Unity.MeshGeneration.Factories
{
    using UnityEngine;
    using Mapbox.Directions;
    using System.Collections.Generic;
    using System.Linq;
    using Mapbox.Unity.Map;
    using Data;
    using Modifiers;
    using Mapbox.Utils;
    using Mapbox.Unity.Utilities;
    using System.Collections;
    using System;
    using UnityEngine.UI;
    using Mapbox.Unity.Location;
    using GetTheLocation;


    public class DirectionsFactory : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        MeshModifier[] MeshModifiers;

        [SerializeField]
        Material _material;

        private List<Vector3> _cachedWaypoints;

        //New Code 

        //[SerializeField]
        //[Geocode]
        
        //[SerializeField]
        //[Geocode]
         private string _FromDestinationCordinates;

         public GameObject _markerPrefab;
        


        //[SerializeField]
        //float _spawnScale = 100f;

        public GameObject ToInputField;
     
        GameObject DirectionGameObjects;



        //List<GameObject> _spawnedObjects;
        private Transform[] _markerPostions;
        private Vector2d[] _locations;
        private List<string> _locationStrings = new List<string>();
        private float UpdateFrequency = 2;
        private float _spawnScale = 2f;

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


        // End of new code


        private Directions _directions;
        private int _counter;

        GameObject _directionsGO;
        private bool _recalculateNext;

        protected void SetMapMarker()
        {
            _locations = new Vector2d[_locationStrings.Count];
            //_spawnedObjects = new List<GameObject>();
            _markerPostions = new Transform[_locationStrings.Count];

            for (int i = 0; i < _locationStrings.Count; i++)
            {
                _locations[i] = Conversions.StringToLatLon(_locationStrings[i]);
                
                //Don't Create Marker for starting postion 

                if( i != 0)
                {
                    var instance = Instantiate(_markerPrefab);
                    instance.transform.SetParent(DirectionGameObjects.transform);
                    instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
                    instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                    _markerPostions[i] = instance.transform;
                    //_spawnedObjects.Add(instance);
                }
                else
                {
                    GameObject UserCurrentLocationGameObject = new GameObject("UserCurrentLocationGameObject");
                    UserCurrentLocationGameObject.transform.SetParent(DirectionGameObjects.transform);
                    var instance = Instantiate(UserCurrentLocationGameObject);
                    instance.transform.SetParent(DirectionGameObjects.transform);
                    instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
                    instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                    _markerPostions[i] = instance.transform;
                    //_spawnedObjects.Add(instance);
                }

            }
        }

        protected virtual void Awake()
        {
    
            if (_map == null)
            {
                _map = FindObjectOfType<AbstractMap>();
            }
            _directions = MapboxAccess.Instance.Directions;
           // _map.OnInitialized += Query;
            _map.OnUpdated += Query;
        }


        public void Start()
        {


            // Navigation Gameboject for the scene          


            DirectionGameObjects = new GameObject("DirectionGameObjects");

            //Initialization of Player current location
            LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;


            // _FromDestinationCordinates = LocationProvider.CurrentLocation.LatitudeLongitude.ToString().Split(',')[1].Trim() + "," + LocationProvider.CurrentLocation.LatitudeLongitude.ToString().Split(',')[0].Trim();
            _FromDestinationCordinates = _GetLocation.GetComponent<GettheLocation>().CurrentLocation;


            _locationStrings.Add(_FromDestinationCordinates);
            _locationStrings.Add(ToInputField.GetComponent<InputField>().text.ToString());

             SetMapMarker();

            _cachedWaypoints = new List<Vector3>(_locationStrings.Count);

            foreach (var item in _markerPostions)
            {

                _cachedWaypoints.Add(item.position);

            }
            _recalculateNext = false;

            foreach (var modifier in MeshModifiers)
            {
                modifier.Initialize();
            }

            StartCoroutine(QueryTimer());
        }


        private void LateUpdate()
        {
            if (_isInitialized)
            {
               // _FromDestinationCordinates = LocationProvider.CurrentLocation.LatitudeLongitude.ToString().Split(',')[1].Trim() + "," + LocationProvider.CurrentLocation.LatitudeLongitude.ToString().Split(',')[0].Trim();
                _FromDestinationCordinates = _GetLocation.GetComponent<GettheLocation>().CurrentLocation;
            }
        }


        private void Update()
        {
            //int count = _spawnedObjects.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    var spawnedObject = _spawnedObjects[i];
            //    spawnedObject.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
            //    spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            //}

        }


        protected virtual void OnDestroy()
        {
            _map.OnInitialized -= Query;
            _map.OnUpdated -= Query;
        }

         public void Query()
        {
            var count = _markerPostions.Length;
            var wp = new Vector2d[count];

            for (int i = 0; i < count; i++)
            {
                wp[i] = _markerPostions[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
            }
            var _directionResource = new DirectionResource(wp, RoutingProfile.Walking);
            _directionResource.Steps = true;
            _directions.Query(_directionResource, HandleDirectionsResponse);
        }

        public IEnumerator QueryTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(UpdateFrequency);
                for (int i = 0; i < _markerPostions.Length; i++)
                {
                    if (_markerPostions[i].position != _cachedWaypoints[i])
                    {
                        _recalculateNext = true;
                        _cachedWaypoints[i] = _markerPostions[i].position;
                    }
                }

                if (_recalculateNext)
                {
                    Query();
                    _recalculateNext = false;
                }
            }
        }

        void HandleDirectionsResponse(DirectionsResponse response)
        {
            if (response == null || null == response.Routes || response.Routes.Count < 1)
            {
                return;
            }

            var meshData = new MeshData();
            var dat = new List<Vector3>();
            foreach (var point in response.Routes[0].Geometry)
            {
                dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
            }

            var feat = new VectorFeatureUnity();
            feat.Points.Add(dat);

            foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
            {
                mod.Run(feat, meshData, _map.WorldRelativeScale);
            }

            CreateGameObject(meshData);
        }

        GameObject CreateGameObject(MeshData data)
        {
            if (_directionsGO != null)
            {
                _directionsGO.Destroy();
            }

            _directionsGO = new GameObject("direction waypoint entity");       
            _directionsGO.transform.SetParent(DirectionGameObjects.transform);
            var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
            mesh.subMeshCount = data.Triangles.Count;

            mesh.SetVertices(data.Vertices);
            _counter = data.Triangles.Count;
            for (int i = 0; i < _counter; i++)
            {
                var triangle = data.Triangles[i];
                mesh.SetTriangles(triangle, i);
            }

            _counter = data.UV.Count;
            for (int i = 0; i < _counter; i++)
            {
                var uv = data.UV[i];
                mesh.SetUVs(i, uv);
            }

            mesh.RecalculateNormals();
            _directionsGO.AddComponent<MeshRenderer>().material = _material;
            return _directionsGO;
        }


        public void ResettheScript()
        {
            try
            {

                _locationStrings.Clear();
                //_spawnedObjects.Clear();
                _markerPostions = new Transform[] { };
                _locations = new Vector2d[] { };

                if( DirectionGameObjects.transform.childCount > 0)
                {
                    foreach (Transform child in DirectionGameObjects.transform)
                    {
                        GameObject.Destroy(child.gameObject);

                    }
                }

                Destroy(DirectionGameObjects.gameObject);

            }
            catch(Exception ex)
            {
                Debug.Log("Exception in ResettheScript" + ex.Message);
            }
        }
    }
 }
