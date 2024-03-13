using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumpet1 : MonoBehaviour
{
    [SerializeField] public AudioClip[] trumpetsamples;
    public AudioSource audioSource;
    private int clipIndex;

    void Start()
    {
       
    }

    public void PlayClip1(int noteIndex)
    {
        clipIndex = noteIndex - 48;
        if (clipIndex < trumpetsamples.Length && clipIndex >= 0)
        {
            audioSource.clip = trumpetsamples[clipIndex];
            Debug.Log("Index"+clipIndex);
            audioSource.Play();
        }
    }

    public void StopClip1()
    {
        audioSource.Stop();
    }
}
