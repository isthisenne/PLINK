using UnityEngine;

public class TitleMusic : MonoBehaviour
{
    public AudioClip music;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
       if(music!=null)
        {
            audioSource.clip = music;
        }
        audioSource.loop = true;
        audioSource.Play();

      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Stop();
        }

    }
}
