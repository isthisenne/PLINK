using System;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameManager.Level targetLevel;

    private void OnMouseDown()
    {
        print("Loading Level: " + targetLevel.ToString());
        GameManager.Instance.LoadLevel(targetLevel);
    }

    // tint when hovered
    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.7f);
    }
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}