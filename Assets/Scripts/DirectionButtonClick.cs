using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DirectionButtonClick : MonoBehaviour
{
     string FromDirectioncorintate;
     string ToDirectioncorintate;

    public GameObject fromInputField;
    public GameObject ToInputField;
    public GameObject textDisplay;


    public void ShowDirection()
    {
        try
        {
            FromDirectioncorintate = fromInputField.GetComponent<InputField>().text.ToString();
            ToDirectioncorintate = ToInputField.GetComponent<InputField>().text.ToString();
            textDisplay.GetComponent<Text>().text = FromDirectioncorintate + "---" + ToDirectioncorintate;

        }
        catch(Exception ex)
        {
            Debug.Log("error is " + ex);
        }
 
    }

}
