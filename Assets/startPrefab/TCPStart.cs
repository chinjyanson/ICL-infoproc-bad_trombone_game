using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TCPStart : MonoBehaviour
{
    public static string playername;
    public static int playerno;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        const string serverName = "13.43.110.192";
        const int serverPort = 14000;

        using (var client = new TcpClient())
        {
            try
            {
                client.Connect(serverName, serverPort);
                Debug.Log("Connected to the server.");
                NetworkStream stream = client.GetStream();
                var data = new { type = "playerselect", username = playername};
                string message = JsonConvert.SerializeObject(data);
                byte[] dataToSend = Encoding.ASCII.GetBytes(message);
                stream.Write(dataToSend, 0, dataToSend.Length);

                //Retrieve data length

                byte[] receivedBytes = new byte[4096];
                int bytesRead = stream.Read(receivedBytes, 0, receivedBytes.Length);
                string info = Encoding.ASCII.GetString(receivedBytes, 0, bytesRead);
                Debug.Log(info);
                if(info == "player1")
                {
                    playerno = 1;
                }
                else if (info == "player2")
                {
                    playerno = 2;
                }
                SceneManager.LoadScene("Loading");
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
