using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class LoadingDisplay : MonoBehaviour
{
    public TMP_Text PlayerName;
    // Start is called before the first frame update
    void Start()
    {
        int playerno = TCPStart.playerno;
        if(playerno == 1)
        {
            PlayerName.text = "You are Player 1";
        } else
        {
            PlayerName.text = "You are Player 2";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
