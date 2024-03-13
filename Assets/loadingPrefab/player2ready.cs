using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class player2ready : MonoBehaviour
{
    MyListener listener;
    // Start is called before the first frame update
    public TMP_Text hello2;
    void Start()
    {
        hello2.text = "Not Ready";
        hello2.color = Color.red;
    }

    void Update()
    {
        if (StartGame.b2start)
        {
            hello2.text = "Ready";
        }
        if (StartGame.b2pressed)
        {
            hello2.color = Color.green;
        }
        else
        {
            hello2.color = Color.red;
        }
    }

    // Update is called once per frame
}

