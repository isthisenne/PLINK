using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set; }
    LevelTransitionStart levelTransition;  // reference to the level transitioner
    
    // LIST OF ALL LEVELS
    // add all new levels here and to build profile
    public enum Level
    {
        Title, Alep_1, Alep_2, Alep_3, Bet_1, Bet_2, Bet_3, Gimel_1, Gimel_2, Gimel_3, Dalet_1, Dalet_2, Dalet_3, 
        He_1, He_2, He_3, Waw_1, Waw_2, Waw_3, Zayin_1, Zayin_2, Zayin_3, Het_1, Het_2, Het_3, Tet_1, Tet_2, Tet_3,
        Yod_1,  Yod_2, Yod_3, Kap_1, Kap_2, Kap_3, Lamed_1, Lamed_2, Lamed_3, Mem_1, Mem_2, Mem_3, Nun_1, Nun_2, Nun_3,
        Samekh_1, Samekh_2, Samekh_3, Ayin_1, Ayin_2, Ayin_3, Pe_1, Pe_2, Pe_3, Sade_1, Sade_2, Sade_3, Qof_1, Qof_2, Qof_3,
        Rosh_1, Rosh_2, Rosh_3, Shin_1, Shin_2, Shin_3
    }

    public Level currentLevel = 0;
    
    private void Awake()
    {
        // singleton
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
        levelTransition = null;
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

            // play level transition if end of stage
            levelTransition = FindObjectOfType<LevelTransitionStart>();
            if (levelTransition != null)
            {
                levelTransition.TransitionToNextLevel();
                return;
            }
            
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
