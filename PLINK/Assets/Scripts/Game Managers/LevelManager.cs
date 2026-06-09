using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Token")]
    public GameObject tokenPrefab;
    public Vector3 tokenSpawnPosition;
    private GameObject token;
    
    [Header("Health UI")]
    public GameObject healthIndicator;
    private List<GameObject> healthIndicators = new List<GameObject>();
    private int health = 3;
    
    [Header("Sound")]
    public AudioClip winSfx;
    public AudioClip failSfx;
    private AudioSource audioSource;
    
    private bool won = false;
    
    private void Start()
    {
        token = Instantiate(tokenPrefab, tokenSpawnPosition, Quaternion.identity);
        InitializeHealthUI();
        audioSource = GetComponent<AudioSource>();
    }

    public void ProgressLevel()
    {
        if (won) return;
        won = true;
        
        Debug.Log("YOU WIN!");
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        audioSource.PlayOneShot(winSfx, 0.5f);
        yield return new WaitForSecondsRealtime(2.1f);
        GameManager.Instance.LoadNextLevel();
    }

    public void Fail()
    {
        health -= 1;
        Debug.Log("Fail, " + health + " life left.");
        if (health == 0)
        {
            GameManager.Instance.RestartGame();
        }
        
        UpdateHealthUI();
        Destroy(token);
        token = Instantiate(tokenPrefab, tokenSpawnPosition, Quaternion.identity);
        audioSource.PlayOneShot(failSfx, 0.5f);
    }

    // initialize health UI with three icons
    private void InitializeHealthUI()
    {
        for (int i = 0; i < health; i++)
        {
            Vector3 position = new Vector3(-65 + (i * 5), 35, 0);
            GameObject icon = Instantiate(healthIndicator, position, Quaternion.identity, transform);
            healthIndicators.Add(icon);
        }
    }

    // remove health icon from UI
    private void UpdateHealthUI()
    {
        Destroy(healthIndicators[health]);
        healthIndicators.RemoveAt(health);
    }
}
