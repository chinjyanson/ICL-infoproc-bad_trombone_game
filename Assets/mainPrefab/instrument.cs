using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class instrument : MonoBehaviour
{   
    // Start is called before the first frame update
    public AudioSource twinkle;
    public AudioSource sym;
    public AudioSource MI;
    public float TwinkleDelay;
    public float SymDelay;
    public float MIDelay;
    public AudioClip c4s;
    public AudioClip d4;

    void Start()
    {
        string song = button1UI.SongChoice;
        if (song == "twinkle") 
        {
            twinkle.PlayDelayed(TwinkleDelay);
        }
        else if (song == "5sym")
        {
            sym.PlayDelayed(SymDelay);
        }
        else if(song == "MissionIm")
        {
            MI.PlayDelayed(MIDelay);
        }  
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
    }
}
