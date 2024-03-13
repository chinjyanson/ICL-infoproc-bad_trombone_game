using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class P1BestScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text displaytext;
    void Start()
    {   
        displaytext.text = TCPscore.P1best;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
