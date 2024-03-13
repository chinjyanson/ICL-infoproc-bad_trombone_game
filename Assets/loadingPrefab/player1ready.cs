using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class player1ready: MonoBehaviour
{
    StartGame listener;
    // Start is called before the first frame update
    public TMP_Text hello;
    void Start()
    {
        hello.text = "Not Ready";
        hello.color = Color.red;
    }

    void Update()
    {
        if (StartGame.b1start)
        {
            hello.text = "Ready";
        }
        if (StartGame.b1pressed)
        {
            hello.color = Color.green;
        }
        else
        {
            hello.color = Color.red;
        }
    }

    // Update is called once per frame
}
