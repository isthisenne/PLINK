using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set; }
    
    // LIST OF ALL LEVELS
    // add all new levels here and to build profile
    public enum Level
    {
        Title, Alep_1, Alep_2, Alep_3, Bet_1, Bet_2, Bet_3, Gimel_1, Gimel_2, Gimel_3
    }

    public Level currentLevel = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        GetCurrentLevel();
    }

    // get current level
    private void GetCurrentLevel()
    {
        currentLevel = (Level)SceneManager.GetActiveScene().buildIndex;
    }

    public string CurrentLevel()
    {
        return currentLevel.ToString();
    }
    
    // loads the current set scene
    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(currentLevel.ToString());
    }

    // loads the next scene
    [ContextMenu("Next Level")]
    public void LoadNextLevel()
    {
        Level nextLevel = currentLevel + 1;

        if (nextLevel < (Level)System.Enum.GetNames(typeof(Level)).Length)
        {
            currentLevel = nextLevel;
            LoadCurrentScene();
        }
        else
        {
            Debug.Log("No more levels");
        }
    }

    public void RestartGame()
    {
        currentLevel = 0;
        SceneManager.LoadScene("TITLE");
    }
}
