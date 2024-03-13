using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class P2BestScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text displaytext;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        displaytext.text = TCPscore.P2best;
    }
}
