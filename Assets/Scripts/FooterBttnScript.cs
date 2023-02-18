using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FooterBttnScript : MonoBehaviour, IPointerClickHandler
{

    public void OnMouseDown()
    {

        GameObject clickedObject = gameObject;

        if (clickedObject.name == "LeaderBoardIconBttn")
        {
            Debug.Log("Bttn Pressed");
            SceneManager.LoadScene("LeaderBorad");
        }

        if (clickedObject.name == "EarthIconBttn")
        {

            SceneManager.LoadScene("FriendsPage");
        }


        if (clickedObject.name == "HomeIconBttn")
        {
            SceneManager.LoadScene("HomePage");
        }


        if (clickedObject.name == "CollectionIconBttn")
        {

            SceneManager.LoadScene("CollectionPage");
        }


        if (clickedObject.name == "ProfileIconBttn")
        {

            SceneManager.LoadScene("LeaderBorad");
        }

    }

         
    public void OnPointerClick(PointerEventData ped)
    {

        Debug.Log("Bttn Pressed");
        
    }





}
