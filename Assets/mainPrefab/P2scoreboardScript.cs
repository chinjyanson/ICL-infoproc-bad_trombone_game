using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class P2scoreboardScript : MonoBehaviour
{
    // Start is called before the first frame update
    public string P2name;
    public static float P2score;



    [SerializeField] public TMP_Text P2scoreboardstring;
    void Start()
    {
        //P1name = inputFieldGrabber.player1NameInputText;
        P2name = PlayerPrefs.GetString("Player2name");
    }

    // Update is called once per frame
    void Update()
    {
        //P1score = HitObject.; // implement getScore in GameManager/Note
        P2scoreboardstring.text = P2name + "       " + P2score; // figure out how to convert from float->string and concatanate 
    }
}