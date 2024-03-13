using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class inputFieldGrabber2 : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Showing the something else")]
    [SerializeField] public string inputText2;
    

    public static string player2NameInputText;

    public void GrabFromInputField(string input)
    {
        inputText2 = input;
        PlayerPrefs.SetString("Player2name", input);
        player2NameInputText = input;
    }

}