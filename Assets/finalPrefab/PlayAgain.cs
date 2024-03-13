using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ResetGame()
    {
        P1scoreboardScript.P1score = 0;
        P2scoreboardScript.P2score = 0;
        TCPscore.P1best = "0";
        TCPscore.P2best = "0";
        TCPscore.leaderNames = new List<string>();
        TCPscore.leaderScores = new List<string>();
        SceneManager.LoadScene("Start");
    }
}
