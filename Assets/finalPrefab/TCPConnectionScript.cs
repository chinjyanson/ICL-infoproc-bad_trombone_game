using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class TCPscore : MonoBehaviour
{
    // Start is called before the first frame update

    public static string P1best;
    public static string P2best;
    public static string P1score;
    public static string P2score;
    public static List<string> leaderNames = new List<string>();
    public static List<string> leaderScores = new List<string>();

    private void Start()
    {
        const string serverName = "13.43.110.192";
        const int serverPort = 14000;

        using (var client = new TcpClient())
        {
            try
            {
                client.Connect(serverName, serverPort);
                NetworkStream stream = client.GetStream();
                //string song = button1UI.SongChoice;
                string song = "Twinkle";
                string username1 = PlayerPrefs.GetString("Player1name");
                string score1 = P1scoreboardScript.P1score.ToString();
                string username2 = PlayerPrefs.GetString("Player2name");
                string score2 = P2scoreboardScript.P2score.ToString();
                int playerno = TCPStart.playerno;
                var scoreData = new { type = "game", song , username1, score1 , username2, score2, playerno };
                string message = JsonConvert.SerializeObject(scoreData);
                byte[] dataToSend = Encoding.ASCII.GetBytes(message);
                stream.Write(dataToSend, 0, dataToSend.Length);

                //Retrieve data length

                byte[] lenBytes = new byte[1024];
                int lenBytesRead = stream.Read(lenBytes, 0, lenBytes.Length);
                int datalen = int.Parse(Encoding.ASCII.GetString(lenBytes, 0, lenBytesRead));

                stream.Write(dataToSend, 0, dataToSend.Length);

                byte[] receivedBytes = new byte[4096];
                int totalBytesRead = 0;
                StringBuilder score = new StringBuilder();
                do
                {
                    int bytesRead = stream.Read(receivedBytes, 0, receivedBytes.Length);
                    totalBytesRead += bytesRead;
                    score.Append(Encoding.ASCII.GetString(receivedBytes, 0, bytesRead));
                } while (totalBytesRead < datalen);
                string scores = score.ToString();
                Debug.Log(scores);
                var jsonArray = JsonConvert.DeserializeObject<List<List<object>>>(scores);
                // Iterate through the parsed JSON array
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    var innerList = jsonArray[i];

                    // Print the elements of the inner list
                    foreach (var element in innerList)
                    {
                        if (element is JArray)  
                        {
                            // Handle inner JArray
                            var innerArray = (JArray)element;
                            Debug.Log($"  Name: {innerArray[0]}, Amount: {innerArray[1]}");
                            leaderNames.Add(innerArray[0].ToString());
                            leaderScores.Add(innerArray[1].ToString());
                        }
                        else
                        {
                            if (i == 1)
                            {
                                P1best = element.ToString();
                                Debug.Log($"Player 1 Score: {element}");

                            }
                            else if (i == 2)
                            {
                                P2best = element.ToString();
                                Debug.Log($"Player 2 Score: {element}");
                            }
                            else if (i == 3)
                            {
                                P1score = element.ToString();
                            }
                            else if (i == 4)
                            {
                                P2score = element.ToString();
                            }
                        }
                    }
                }
                stream.Write(dataToSend, 0, dataToSend.Length);
                client.Close();
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException: " + se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception: " + e.Message);
            }
        }
    }
}
