using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class P1scoreboardScript : MonoBehaviour
{
    // Start is called before the first frame update
    public string P1name;
    public static float P1score;



    [SerializeField] public TMP_Text P1scoreboardstring;
    void Start()
    {
        //P1name = inputFieldGrabber.player1NameInputText;
        P1name = PlayerPrefs.GetString("Player1name");
    }

    // Update is called once per frame
    void Update()
    {
        //P1score = HitObject.; // implement getScore in GameManager/Note
        P1scoreboardstring.text = P1name + "       " + P1score; // figure out how to convert from float->string and concatanate 
    }
}