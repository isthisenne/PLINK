using System;
using UnityEngine;

public class LevelTransitionEnd : MonoBehaviour
{
    [Header("Scene References")]
    public SpriteRenderer background;
    public GameObject letter;
    
    [Header("Transition Settings")]
    public float pauseLength = 1f; // time for scene to wait
    public float enterLength = 1f; // time it takes for letter to fly off
    
    // acceleration modifier for movement; 1 is normal acceleration, 2 is quadratic curve
    [SerializeField, Range(0.1f, 3f)] private float enterAcceleration = 1.5f; 
    
    private SpriteRenderer letter_sprite;
    private Shader shaderGUItext;
    private Vector3 startPosition;
    private Vector3 enterDestination = new Vector3(0, 0, 0);
    
    public bool transitioning = false;
    private float pauseTime = 0f;
    private float enterTime = 0f;

    private void Start()
    {
        startPosition = letter.transform.position;
        letter_sprite = letter.GetComponent<SpriteRenderer>();
        
        // set letter to white
        shaderGUItext = Shader.Find("GUI/Text Shader");
        letter_sprite.material.shader = shaderGUItext;
        letter_sprite.color = new Color(1f, 1f, 1f, 1f);
        
        SetAlpha(1f); // display transition visuals
    }

    private void Update()
    {
        // handle background and letter fade-in
        enterTime += Time.deltaTime;
        float enterProgress = Mathf.Clamp01(enterTime / enterLength);
        float easedProgress = Mathf.Pow(enterProgress, enterAcceleration);
        
        letter.transform.position = Vector3.Lerp(startPosition, enterDestination, easedProgress);
        
        // handle letter exit off-screen
        if (enterProgress >= 1f)
        {   
            pauseTime += Time.deltaTime;
            float pauseProgress = Mathf.Clamp01(pauseTime / pauseLength);
            
            if (pauseProgress >= 1f)
            {
                // Level begins
                SetAlpha(0f);
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
        letter_sprite.color = new Color(letter_sprite.color.r, letter_sprite.color.g, letter_sprite.color.b, alpha);
    }
}
