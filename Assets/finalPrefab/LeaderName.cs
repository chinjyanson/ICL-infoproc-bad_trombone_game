using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class LeaderName : MonoBehaviour
{
    public TMP_Text displaytext1;
    public TMP_Text displaytext2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StringBuilder leadernames = new StringBuilder();
        foreach (string name in TCPscore.leaderNames)
        {
            leadernames.Append(name);
            leadernames.AppendLine();
        }
        displaytext1.text = leadernames.ToString();

        StringBuilder leaderscores = new StringBuilder();
        foreach (string name in TCPscore.leaderScores)
        {
            leaderscores.Append(name);
            leaderscores.AppendLine();
        }
        displaytext2.text = leaderscores.ToString();
    }
}
