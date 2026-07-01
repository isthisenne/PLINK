using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Songs - Add here in prefab")]
    public AudioClip[] songs; // where you add songs in the inspector (in order)
    [Header("Dont Touch")]
    private int level = 0;

    public AudioSource GetAS()
    {
        return GetComponent<AudioSource>();
    }

    public void NextSong()
    {
        AudioSource audioSource = GetAS();
        audioSource.Pause();

        if (level + 1 < songs.Length)
            audioSource.clip = songs[level++];
    }
}
