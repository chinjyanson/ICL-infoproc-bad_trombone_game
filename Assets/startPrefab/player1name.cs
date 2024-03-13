using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class inputFieldGrabber : MonoBehaviour
{

    // Start is called before the first frame update
    [Header("Showing the something else")]
    [SerializeField] public string inputText;
    public static string playerNameInputText;

    public void GrabFromInputField(string input)
    {
        inputText = input;
        Debug.Log("Name is" + inputText);
        TCPStart.playername = inputText;
        playerNameInputText = input;
    }

}
