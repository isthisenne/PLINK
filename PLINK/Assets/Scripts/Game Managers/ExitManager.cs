using System;
using TMPro;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float timeToQuit = 2.0f;
    
    private float escapeTimer = 0f;
    
    // close game if escape key is held for 2 seconds
    private void Update()
    {
        if (escapeTimer >= timeToQuit)
        {
            QuitGame();
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            escapeTimer += Time.deltaTime;
            int exitProgress = (int)((escapeTimer / timeToQuit) * 3); // go from 0 to 3 dots
            string dots = new string('.', exitProgress);
            text.text = "Exiting" + dots;
        }
        else
        {
            text.text = "";
            escapeTimer = 0f;
        }
    }
    
    private void QuitGame()
    {
        //TODO: add save data feature here
        
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif
    }
}
