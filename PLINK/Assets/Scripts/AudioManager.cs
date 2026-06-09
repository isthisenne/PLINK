using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Songs - Add here in prefab")]
    public AudioClip[] songs; // where you add songs in the inspector (in order)
    [Header("Dont Touch")]
    public AudioSource audioSource;
    private int level = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void NextSong()
    {
        audioSource.Pause();

        if (level + 1 < songs.Length)
            audioSource.clip = songs[level++];
    }
}
