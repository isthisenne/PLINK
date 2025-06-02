using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // draw default options
        DrawDefaultInspector();
        
        GameManager gameManager = (GameManager)target;

        // add button to progress level
        if (GUILayout.Button("Next Level"))
        {
            gameManager.LoadNextLevel();
        }
        
        // add button to restart levels
        if (GUILayout.Button("Restart Game"))
        {
            gameManager.RestartGame();
        }
    }
}
