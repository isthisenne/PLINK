using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip startSfx;
    
    [Header("Title")]
    [SerializeField] private GameObject title;
    [SerializeField] private float scalingRate = 0.1f;
    
    private bool started = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            started = true;
            StartCoroutine(StartGame());
        }

        if (started)
        {
            float newScale = title.transform.localScale.x + scalingRate * Time.deltaTime;
            title.transform.localScale = Vector3.one * newScale;
        }
    }

    private IEnumerator StartGame()
    {
        audioSource.Pause();
        audioSource.clip = startSfx;
        audioSource.Play();
        
        yield return new WaitForSecondsRealtime(4f);
        GameManager.Instance.LoadNextLevel();
    }
}
