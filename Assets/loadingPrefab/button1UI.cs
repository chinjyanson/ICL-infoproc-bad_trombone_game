using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button1UI : MonoBehaviour
{
    // Start is called before the first frame update

    private void Start()
    {
    }

    public TMP_Text SongSelect;
    public static string musicDataReceived;
    public static string SongChoice;

    public void TwinkleButton()
    {
        const string serverName = "13.43.110.192";
        const int serverPort = 14000;
        int playerno = TCPStart.playerno;

        SongChoice = "twinkle";

        using (var client = new TcpClient())
        {
            try
            {
                client.Connect(serverName, serverPort);
                Debug.Log("Connected to the server.");
                NetworkStream stream = client.GetStream();
                var musicData = new { type = "music", name = "twinkle", author = "yues", player = playerno };
                string message = JsonConvert.SerializeObject(musicData);
                byte[] dataToSend = Encoding.ASCII.GetBytes(message);
                stream.Write(dataToSend, 0, dataToSend.Length);
                
                //Retrieve data length

                byte[] lenBytes = new byte[1024];
                int lenBytesRead=stream.Read(lenBytes, 0, lenBytes.Length);
                int datalen = int.Parse(Encoding.ASCII.GetString(lenBytes, 0, lenBytesRead));
                Debug.Log(datalen.ToString());
                stream.Write(dataToSend, 0, dataToSend.Length);

                byte[] receivedBytes = new byte[4096]; 
                int totalBytesRead = 0;
                StringBuilder notes = new StringBuilder();
                do
                {
                    int bytesRead = stream.Read(receivedBytes, 0, receivedBytes.Length);
                    totalBytesRead += bytesRead;
                    notes.Append(Encoding.ASCII.GetString(receivedBytes, 0, bytesRead));
                } while (totalBytesRead<datalen);


                musicDataReceived = notes.ToString();
                musicDataReceived = musicDataReceived.Substring(1, musicDataReceived.Length - 2);
                Debug.Log(musicDataReceived);
                stream.Write(dataToSend, 0, dataToSend.Length);
                client.Close();
                SongSelect.text = "You Chose Twinkle";
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

    public void SymButton()
    {
        const string serverName = "13.43.110.192";
        const int serverPort = 14000;
        int playerno = TCPStart.playerno;

        SongChoice = "5sym";

        using (var client = new TcpClient())
        {
            try
            {
                client.Connect(serverName, serverPort);
                Debug.Log("Connected to the server.");
                NetworkStream stream = client.GetStream();
                var musicData = new { type = "music", name = "5sym", author = "Unknown", player = playerno };
                string message = JsonConvert.SerializeObject(musicData);
                byte[] dataToSend = Encoding.ASCII.GetBytes(message);
                stream.Write(dataToSend, 0, dataToSend.Length);

                //Retrieve data length

                byte[] lenBytes = new byte[1024];
                int lenBytesRead = stream.Read(lenBytes, 0, lenBytes.Length);
                int datalen = int.Parse(Encoding.ASCII.GetString(lenBytes, 0, lenBytesRead));
                Debug.Log(datalen.ToString());
                stream.Write(dataToSend, 0, dataToSend.Length);

                byte[] receivedBytes = new byte[4096];
                int totalBytesRead = 0;
                StringBuilder notes = new StringBuilder();
                do
                {
                    int bytesRead = stream.Read(receivedBytes, 0, receivedBytes.Length);
                    totalBytesRead += bytesRead;
                    notes.Append(Encoding.ASCII.GetString(receivedBytes, 0, bytesRead));
                } while (totalBytesRead<datalen);

                musicDataReceived = notes.ToString();
                musicDataReceived = musicDataReceived.Substring(1, musicDataReceived.Length - 2);
                Debug.Log(musicDataReceived);
                stream.Write(dataToSend, 0, dataToSend.Length);
                client.Close();
                SongSelect.text = "You Chose 5Sym";
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

    public void MIButton()
    {
        const string serverName = "13.43.110.192";
        const int serverPort = 14000;
        int playerno = TCPStart.playerno;

        SongChoice = "MissionIm";

        using (var client = new TcpClient())
        {
            try
            {
                client.Connect(serverName, serverPort);
                Debug.Log("Connected to the server.");
                NetworkStream stream = client.GetStream();
                var musicData = new { type = "music", name = "MissionIm", author = "Unknown", player = playerno };
                string message = JsonConvert.SerializeObject(musicData);
                byte[] dataToSend = Encoding.ASCII.GetBytes(message);
                stream.Write(dataToSend, 0, dataToSend.Length);

                //Retrieve data length

                byte[] lenBytes = new byte[1024];
                int lenBytesRead = stream.Read(lenBytes, 0, lenBytes.Length);
                int datalen = int.Parse(Encoding.ASCII.GetString(lenBytes, 0, lenBytesRead));
                Debug.Log(datalen.ToString());
                stream.Write(dataToSend, 0, dataToSend.Length);

                byte[] receivedBytes = new byte[4096]; // Assuming music data can be larger
                int totalBytesRead = 0;
                StringBuilder notes = new StringBuilder();
                do
                {
                    int bytesRead = stream.Read(receivedBytes, 0, receivedBytes.Length);
                    totalBytesRead += bytesRead;
                    notes.Append(Encoding.ASCII.GetString(receivedBytes, 0, bytesRead));
                } while (totalBytesRead<datalen);

                
                musicDataReceived = notes.ToString();
                musicDataReceived = musicDataReceived.Substring(1,musicDataReceived.Length - 2);
                Debug.Log(musicDataReceived);
                stream.Write(dataToSend, 0, dataToSend.Length);
                client.Close();
                SongSelect.text = "You Chose MissionIm";
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
