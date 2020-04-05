using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public List<AudioClip> loops;
    public AudioSource audioSource;
    
    void Start()
    {
        audioSource.clip = loops[Random.Range(0, loops.Count)];
        audioSource.Play();
    }
}
