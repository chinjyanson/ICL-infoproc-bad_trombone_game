using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using TMPro;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    Thread thread;
    public int connectionPort = 14000;
    public static bool b1start = false;
    public static bool b1pressed = false;
    public static bool b2start = false;
    public static bool b2pressed = false;
    public string osong = "nil";
    public bool samesong = false;
    public TMP_Text OtherSong;
    public TMP_Text Warning;
    public int playerno = TCPStart.playerno;
    public string oplayername = "nil";
    


    void Start()
    {
        // Receive on a separate thread so Unity doesn't freeze waiting for data;
        ThreadStart ts = new ThreadStart(GetData);
        thread = new Thread(ts);
        thread.Start();
        Debug.Log(playerno);

    }

    private void Update()
    {
        if (osong == "twinkle")
        {
            OtherSong.text = "Other player chose Twinkle";
        }
        else if (osong == "5sym")
        {
            OtherSong.text = "Other player chose 5Sym";
        }
        else if (osong == "MissionIm")
        {
            OtherSong.text = "Other player chose MissionIm";
        }

        if(osong == "nil")
        {
            samesong = false;
            Warning.text = " ";
        }else if(osong == button1UI.SongChoice)
        {
            Warning.text = "Hold button to get ready!";
            samesong = true;
        } else
        {
            Warning.text = "You need to choose the same song!";
            samesong = false;
        }

        if (TCPStart.playerno == 1)
        {
            PlayerPrefs.SetString("Player1name", TCPStart.playername);
            PlayerPrefs.SetString("Player2name", oplayername);
        }
        else if (TCPStart.playerno == 2)
        {
            PlayerPrefs.SetString("Player2name", TCPStart.playername);
            PlayerPrefs.SetString("Player1name", oplayername);
        }

        if (b1start && b2start && samesong)
        {
            b1start = false;
            b2start = false;
            osong = "nil";
            SceneManager.LoadScene("main game");
        }

    }

    void GetData()
    {
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr = IPAddress.Parse("13.43.110.192");
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 14000);
        Socket client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        

        try
        {
            client.Connect(localEndPoint);
            Debug.Log("Connected to the server.");
            int p1count = 0;
            int p2count = 0;
            var gameData = new { type = "menu", player = TCPStart.playerno};
            string message = JsonConvert.SerializeObject(gameData);
            byte[] dataToSend = Encoding.ASCII.GetBytes(message);
            client.Send(dataToSend);
            do
            {
                byte[] receivedBytes = new byte[1024];
                int bytesRead = client.Receive(receivedBytes);
                string receivedData = Encoding.ASCII.GetString(receivedBytes, 0, bytesRead);
                var jsonArray = JsonConvert.DeserializeObject<List<bool>>(receivedData);
                dataToSend = Encoding.ASCII.GetBytes("menu");
                client.Send(dataToSend);
                byte[] songBytes = new byte[1024];
                int songRead = client.Receive(songBytes);
                string receivedSong = Encoding.ASCII.GetString(songBytes, 0, songRead);
                var jsonSong = JsonConvert.DeserializeObject<List<string>>(receivedSong);
                osong = jsonSong[0];
                oplayername = jsonSong[1];


                if (jsonArray[0])
                {
                    b1pressed = true;
                    //Hold for a few frames to start game. Can change the color of text in the else
                    if (p1count > 50)
                    {
                        b1start = true;
                        p1count = 0;
                    }
                    else
                    {
                        p1count++;
                    }
                }
                else
                {
                    p1count = 0;
                    b1start = false;
                    b1pressed = false;
                }

                if (jsonArray[1])
                {
                    b2pressed = true;
                    //Hold for a few frames to start game. Can change the color of text in the else
                    if (p2count > 50)
                    {
                        b2start = true;
                        p2count = 0;
                    }
                    else
                    {
                        p2count++;
                    }
                }
                else
                {
                    p2count = 0;
                    b2start = false;
                    b2pressed = false;
                }
                
                client.Send(dataToSend);
            } while (!b1start || !b2start||!samesong);
            byte[] sendEnd = Encoding.ASCII.GetBytes("start");
            client.Send(sendEnd);
            client.Close();
            Debug.Log("Game Start");

        }
        catch (SocketException se)
        {
            Debug.Log("SocketException: " + se.Message);
        }
        catch (Exception e)
        {
            Debug.Log("Unexpected exception: " + e.Message);
        }
    }

}