using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

using static GameManager;

public class GameManager : MonoBehaviour
{
    public GameObject notePrefab;
    public GameObject notePrefab2;
    public GameObject hitobject;
    public GameObject hitobject2;
    public GameObject hitObject;
    public GameObject hitObject2;
    public Trumpet trumpetComponent;
    public Trumpet1 trumpetComponent1;
    MyListener listener;
    public TextAsset jsonFile; // Assuming you have a JSON file added to your Unity project
    public TrackData trackData;
    private List<NoteData> noteDataList = new List<NoteData>();
    public static bool EndGame = false;
    public static GameManager Instance { get; private set; }

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    public void Start()
    {
        ParseJsonData(button1UI.musicDataReceived);
        hitObject=Instantiate(hitobject, new Vector3(-8, 0, 0), Quaternion.identity);
        hitObject2 = Instantiate(hitobject2, new Vector3(-8, 0, 0), Quaternion.identity);
        // temporarily shifting position along x-axis since y axis is completely predetermined by mouse location right now (will change after implementing accelerometer)
        StartCoroutine(SpawnNotes());
    }

    void Update()
    {
        SpriteRenderer h1sprite = hitObject.GetComponent<SpriteRenderer>();
        SpriteRenderer h2sprite = hitObject2.GetComponent<SpriteRenderer>();

        if (MyListener.b1)
        {
            h1sprite.color = new Color(255, 0, 0);
        }
        else
        {
            h1sprite.color = new Color(122.0f/255.0f, 19.0f / 255.0f, 19.0f / 255.0f);
        }
        if (MyListener.b2)
        {
            h2sprite.color = new Color(0, 255, 0);
        }
        else
        {
            h2sprite.color = new Color(21.0f / 255.0f, 90.0f / 255.0f, 79.0f / 255.0f);
        }
    }

    public void PlayTrumpetSound(int noteIndex)
    {
        Debug.Log("note index play ts: " + noteIndex);
        trumpetComponent.PlayClip(noteIndex);
    }

    public void StopTrumpetSound()
    {
        trumpetComponent.StopClip(); 
    }

    public void PlayTrumpetSound1(int noteIndex)
    {
        Debug.Log("note index play ts: " + noteIndex);
        trumpetComponent1.PlayClip1(noteIndex);
    }

    public void StopTrumpetSound1()
    {
        trumpetComponent1.StopClip1();
    }

    void ParseJsonData(string jsonString)
    {
        Debug.Log("Parsing JSON Data");
        // Deserialize the JSON string into the TrackData object
        trackData = JsonUtility.FromJson<TrackData>(jsonString);

        noteDataList = trackData.notes;
    }


    IEnumerator SpawnNotes()
    {
        
        float bpm = trackData.tempo;
        float secondsPerBeat = 60f / bpm;
        float elapsedTime = 0f;
        foreach (var noteData in noteDataList)
        {
            float nextNoteTime = noteData.notePosition * secondsPerBeat;
            float waitTime = nextNoteTime - elapsedTime;
            waitTime = Mathf.Max(0, waitTime);
            yield return new WaitForSeconds(waitTime); 
            SpawnNote(noteData);

            // Update the elapsed time
            elapsedTime += waitTime;
        }
        yield return new WaitForSeconds(17);
        EndGame = true;
        SceneManager.LoadScene("Final");
    }

    void SpawnNote(NoteData noteData)
    {
        GameObject noteObject = Instantiate(notePrefab, CalculateSpawnPosition(), Quaternion.identity);
        GameObject noteObject2 = Instantiate(notePrefab2, CalculateSpawnPosition(), Quaternion.identity);
        Note noteComponent = noteObject.GetComponent<Note>();
        Note1 noteComponent2 = noteObject2.GetComponent<Note1>();
        noteComponent.Setup(noteData.noteLength, noteData.startPitch, noteData.pitchDelta, noteData.pitchEnd);
        noteComponent2.Setup1(noteData.noteLength, noteData.startPitch + 100 , noteData.pitchDelta, noteData.pitchEnd+ 100);
        noteComponent.SetSpeedBasedOnTempo(trackData.tempo);
        noteComponent2.SetSpeedBasedOnTempo1(trackData.tempo);
    }


    Vector3 CalculateSpawnPosition()
    {
   
        Vector3 screenRightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, 0));
       
        screenRightEdge.z = 0;

        // Optionally, add a small buffer if you want the note to start exactly at the edge
        // This is useful if the pivot of the sprite is in the center
        float buffer = 0f; // Adjust the buffer as necessary

        // Create the spawn position just off the right edge of the screen
        Vector3 spawnPosition = new Vector3(screenRightEdge.x + buffer, screenRightEdge.y, screenRightEdge.z);

        return spawnPosition;
    }

    public float GetBPM()
    {
        return trackData.tempo; // Assuming 'tempo' holds the BPM value in TrackData
    }
 

    [Serializable]
    public class TrackData
    {
        public List<NoteData> notes;
        public string year;
        public int difficulty;
        public string description;
        public string name;
        public string genre;
        public int tempo;
        public string author;
         // This will hold the notes data
    }

    [Serializable]
    public class NoteData
    {
        public float notePosition;
        public float noteLength;
        public float startPitch;
        public float pitchDelta;
        public float pitchEnd;
    }
}

