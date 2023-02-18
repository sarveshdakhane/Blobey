using UnityEngine;
using Firebase.Database;
using Firebase;
using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;


public class DataBaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;
    private int userID;

    void Start()
    {
            //System.Uri URL = new System.Uri("https://console.firebase.google.com/u/0/project/blobey-ee93d/database/blobey-ee93d-default-rtdb/data/~2F");
            //FirebaseApp.DefaultInstance.Options.DatabaseUrl = URL;

            dbReference = FirebaseDatabase.DefaultInstance.GetReference("blobey_data");
            //dbReference = FirebaseDatabase.DefaultInstance.GetReference("App Data").Child("User Details");
            userID = 1;
            //StartCoroutine("GetUserDetails");
            //StartCoroutine("CheckUserlogin");
            StartCoroutine("CheckNearbyUser");
    }


    void GetUserDetails(string user_id)
    {
        try
        {


            Query query = dbReference.Child("user").Child(user_id);


            query.GetValueAsync().ContinueWith(task =>
            {
     
                if (task.IsFaulted)
                {
               
                 Debug.LogError("query failed ->" + task.Exception);

                }

                else if (task.IsCompleted)
                {
  
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("task.result -> " + task.Result);

                    string Email = snapshot.Child("Email").Value.ToString();
                    Debug.Log("Email: " + Email);


                    foreach (DataSnapshot childSnapshot in snapshot.Child("Post").Children)
                    {
                        if (childSnapshot.Key == "post_01")
                        {
                            GetPostDetails(childSnapshot.Key.ToString());
                        }

                    }
                }
            });
        }


        catch (Exception ex)

        {
            Debug.Log("Exception -> " + ex.Message);

        }
    }


    void GetPostDetails(string post_id)
    {

        Query query = dbReference.Child("post").Child(post_id);


        query.GetValueAsync().ContinueWith(task =>
        {


            if (task.IsFaulted)
            {

                Debug.LogError("query failed ->" + task.Exception);

            }

            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;
                //Debug.Log("task.result -> " + task.Result);


                string content = snapshot.Child("Content").Value.ToString();
                Debug.Log("content -> " + content);
            }
        }


       ); }
     

    void CheckUserlogin()
    {
        try
        {


            Query query = dbReference.Child("user").OrderByChild("Email").EqualTo("sarveshdak@gmail.com").OrderByChild("Password").EqualTo("admin");



            query.GetValueAsync().ContinueWith(task =>
            {

                if (task.IsFaulted)
                {

                    Debug.LogError("query failed ->" + task.Exception);

                }

                else if (task.IsCompleted)
                {

                    DataSnapshot snapshot = task.Result;

                    foreach (DataSnapshot childSnapshot in snapshot.Children)
                    {
                        string userID = childSnapshot.Key;
                        GetUserDetails(userID);

                    }

                }
            });
        }


        catch (Exception ex)

        {
            Debug.Log("Exception -> " + ex.Message);

        }
    }

    void CheckNearbyUser()
    {
        try
        {

            string userLatitude = "50.9003342519417";
            string userLongitude = "8.047429754023703";
            double radius = 100;

            double degrees = radius / 111120;
            double minLat = Convert.ToDouble(userLatitude) - degrees;
            double maxLat = Convert.ToDouble(userLatitude) + degrees;
            double minLon = Convert.ToDouble(userLongitude) - degrees;
            double maxLon = Convert.ToDouble(userLongitude) + degrees;


            Query query = dbReference.Child("user").OrderByChild("latitude").StartAt(minLat).EndAt(maxLat);

            // Attach a listener to the query
            query.GetValueAsync().ContinueWith(task =>
            {

                if (task.IsFaulted)
                {

                    Debug.LogError("query failed ->" + task.Exception);

                }

                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("sss ->" + task.Result.GetRawJsonValue());
                    List<User> nearbyUsers = new List<User>();

                    foreach (DataSnapshot childSnapshot in snapshot.Children)
                    {
                        User user = new User();
                        user.user_id = childSnapshot.Child("ID").Value.ToString();
                        user.latitude = Convert.ToDouble(childSnapshot.Child("latitude").Value);
                        user.longitude = Convert.ToDouble(childSnapshot.Child("longitude").Value);
                        if (user.longitude >= minLon && user.longitude <= maxLon)
                        {
                            nearbyUsers.Add(user);
                        }
                     }
                // Do something with the nearby user data
                Debug.Log(nearbyUsers.Count + " nearby users found!");
                }

            });

        }
        catch (Exception ex)
        {
            Debug.Log("Exception is in CheckNearbyUser ->" + ex.Message);
        }
     }
        
    public class User
    {
        public string user_id;
        public double latitude;
        public double longitude;
    }


}

