using System;
using UnityEngine;

public class LevelTransitionStart : MonoBehaviour
{
    [Header("Scene References")]
    public SpriteRenderer background;
    public GameObject letter;
    
    [Header("Transition Settings")]
    public float pauseLength = 1f; // time for scene to wait
    public float exitLength = 1f; // time it takes for letter to fly off
    
    // acceleration modifier for movement; 1 is normal acceleration, 2 is quadratic curve
    [SerializeField, Range(0.1f, 3f)] private float exitAcceleration = 1.5f; 
    
    private SpriteRenderer letter_sprite;
    private Shader shaderGUItext;
    private Vector3 startPosition;
    private Vector3 exitDestination = new Vector3(-88f, 0, 0);
    
    public bool transitioning = false;
    private float pauseTime = 0f;
    private float exitTime = 0f;

    private void Start()
    {
        startPosition = letter.transform.position;
        letter_sprite = letter.GetComponent<SpriteRenderer>();
        
        // set letter to white
        shaderGUItext = Shader.Find("GUI/Text Shader");
        letter_sprite.material.shader = shaderGUItext;
        letter_sprite.color = new Color(1f, 1f, 1f, 0f);
    }

    private void Update()
    {
        if (!transitioning) return;
        SetVisible();
    
        // handle background and letter fade-in
        pauseTime += Time.deltaTime;
        float pauseProgress = Mathf.Clamp01(pauseTime / pauseLength);
        
        // handle letter exit off-screen
        if (pauseProgress >= 1f)
        {   
            exitTime += Time.deltaTime;
            float exitProgress = Mathf.Clamp01(exitTime / exitLength);
            float easedProgress = Mathf.Pow(exitProgress, exitAcceleration);
            
            letter.transform.position = Vector3.Lerp(startPosition, exitDestination, easedProgress);

            if (exitProgress >= 1f)
            {
                // play next scene
                GameManager.Instance.LoadCurrentScene();
            }
        }
    }

    private void SetVisible()
    {
        background.color = new Color(background.color.r, background.color.g, background.color.b, 1);
        letter_sprite.color = new Color(letter_sprite.color.r, letter_sprite.color.g, letter_sprite.color.b, 1);
    }

    // call to begin level transition
    public void TransitionToNextLevel()
    {
        if(transitioning) return;
        transitioning = true;
        SetVisible();
    }
}
