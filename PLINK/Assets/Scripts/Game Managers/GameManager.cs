using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    LevelTransitionStart levelTransition;  // reference to the level transitioner
    
    // LIST OF ALL LEVELS
    // add all new levels here and to build profile
    public enum Level
    {
        Title, LevelSelect,
        Alep_1, Alep_2, Alep_3, Bet_1, Bet_2, Bet_3, Gimel_1, Gimel_2, Gimel_3,
        Dalet_1, Dalet_2, Dalet_3, He_1, He_2, He_3, Waw_1, Waw_2, Waw_3,
        Zayin_1, Zayin_2, Zayin_3, Het_1, Het_2, Het_3, Tet_1, Tet_2, Tet_3,
        Yod_1, Yod_2, Yod_3, Kap_1, Kap_2, Kap_3, Lamed_1, Lamed_2, Lamed_3,
        Mem_1, Mem_2, Mem_3, Nun_1, Nun_2, Nun_3, Samekh_1, Samekh_2, Samekh_3,
        Ayin_1, Ayin_2, Ayin_3, Pe_1, Pe_2, Pe_3, Sade_1, Sade_2, Sade_3,
        Qof_1, Qof_2, Qof_3, Rosh_1, Rosh_2, Rosh_3, Shin_1, Shin_2, Shin_3
    }

    private static readonly int LevelCount = Enum.GetNames(typeof(Level)).Length;
    public Level CurrentLevel { get; private set; }

    private void Start() => SyncCurrentLevel();
    public string CurrentLevelName() => CurrentLevel.ToString();
    
    // loads the current set scene
    public void LoadCurrentScene()
    {
        levelTransition = null;
        SceneManager.LoadScene(CurrentLevel.ToString());
    }

    // loads the next scene
    [ContextMenu("Next Level")]
    public void LoadNextLevel()
    {
        CurrentLevel++;
        StartLevel();
    }
    
    public void LoadLevel(Level target)
    {
        CurrentLevel = target;
        StartLevel();
    }

    public void RestartGame()
    {
        LoadLevel(Level.LevelSelect);
    }
    
    private void SyncCurrentLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        CurrentLevel = (Level)index;
    }
    
    private void StartLevel()
    {
        levelTransition = FindObjectOfType<LevelTransitionStart>();
        if (levelTransition != null)
        {
            levelTransition.TransitionToNextLevel();
            return;
        }

        LoadCurrentScene();
    }
}
