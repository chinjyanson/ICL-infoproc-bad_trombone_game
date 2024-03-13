using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;



public class HitObject : MonoBehaviour
{
    private string receivedData;
    MyListener listener;
    // Start is called before the first frame update
    private bool hitting; 
    
    void Start()
    {

    }

    void Update()
    {
        // Convert the mouse position from screen space to world space

        float inputMin = 0;
        float inputMax = 512;

        float outputMin = -5;
        float outputMax = 5;

        float final = outputMin + ((MyListener.pos1 - inputMin) / (inputMax - inputMin)) * (outputMax - outputMin);

        // Update the hit zone's position to match the mouse's X position
        // Keep the hit zone's original Y and Z position
        transform.position = new Vector3(transform.position.x,final,transform.position.z);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Note")&&MyListener.b1&&!hitting)
        {
            Note noteScript = other.GetComponent<Note>();
            if (noteScript != null)
            {
                noteScript.HandleHitStart();
                if(TCPStart.playerno == 1)
                {
                    P1scoreboardScript.P1score = Note.getScore();
                }
                hitting = true;
            }
        }
        if (other.CompareTag("Note") && !MyListener.b1&& hitting)
        {
            Note noteScript = other.GetComponent<Note>();
            if (noteScript != null)
            {
                noteScript.HandleHitEnd();
                hitting = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Note")&&hitting)
        {
            Note noteScript = other.GetComponent<Note>();
            if (noteScript != null)
            {
                noteScript.HandleHitEnd();
                hitting = false; 
            }
        }
    }


}
