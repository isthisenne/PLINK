using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject tokenPrefab;
    public Vector3 tokenSpawnPosition;
    private GameObject token;
    
    public GameObject healthIndicator;
    private List<GameObject> healthIndicators = new List<GameObject>();
    private int health = 3;

    private void Start()
    {
        token = Instantiate(tokenPrefab, tokenSpawnPosition, Quaternion.identity);
        InitializeHealthUI();
    }

    public void ProgressLevel()
    {
        Debug.Log("YOU WIN!");
        SceneManager.LoadScene("TITLE");
    }

    public void Fail()
    {
        Debug.Log("Fail, " + health + " life left.");
        health -= 1;
        if (health == 0)
        {
            SceneManager.LoadScene("TITLE");
        }
        
        UpdateHealthUI();
        Destroy(token);
        token = Instantiate(tokenPrefab, tokenSpawnPosition, Quaternion.identity);
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
