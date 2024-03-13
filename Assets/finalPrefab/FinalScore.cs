using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text displaytext1;
    public TMP_Text displaytext2;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        displaytext1.text = TCPscore.P1score;
        displaytext2.text = TCPscore.P2score;
    }
}
