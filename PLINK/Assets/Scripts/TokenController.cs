using System;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    private LevelManager levelManager;
    public float moveSpeed = 10f;
    private Rigidbody2D rb2d;
    private bool isDropped = false;
    private bool useMouseControls = true; // whether input is using mouse or keyboard controls
    
    private AudioSource audioSource;
    public float timeBetweenBounceSFX = 0.1f;
    private float lastBounceTime = -Mathf.Infinity;
    public AudioClip bounce;
    
    // used for secondary loss condition
    private float lastYPos = -100f;
    private float yThreshold = 3f;
    public float heightCheckInterval = 2f;
    private float heightCheckTimer = 0f;
    
    private bool won = false;

    // the Start() function will handle level-specific modifiers to the token
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        // Cursor.visible = false;
        
        // *** LEVEL-SPECIFIC MODIFIERS ***
        
        // bet size modifier
        if (GameManager.Instance.CurrentLevelName().Contains("Bet"))
            this.transform.localScale = new Vector3(0.5f, 0.5f);
        else
            this.transform.localScale = Vector3.one;
        
        // gimel bounce modifier
        PhysicsMaterial2D physicsMaterial = new PhysicsMaterial2D();
        physicsMaterial.friction = 0.4f;
        physicsMaterial.bounciness = 0.3f;
        
        if (GameManager.Instance.CurrentLevelName().Contains("Gimel"))  physicsMaterial.bounciness = 0.8f;
        if (GameManager.Instance.CurrentLevelName().Contains("Tet"))  physicsMaterial.bounciness = 0.0f;
        
        GetComponent<CircleCollider2D>().sharedMaterial = physicsMaterial;
    }

    private void Update()
    {
        // handle token physics
        if (!isDropped)
        {
            HandleMovement();
        }
        else
        {
            heightCheckTimer += Time.deltaTime;
            if (heightCheckTimer >= heightCheckInterval)
            {
                heightCheckTimer = 0f;
                CheckFalling(transform.position.y);
            }
        }
        
        // check input style
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            useMouseControls = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            useMouseControls = true;
        }
    }

    private void HandleMovement()
    {
        // move token based on horizontal input
        if (!useMouseControls)
        {
            float moveHorizontal = Input.GetAxis ("Horizontal");
            if(GameManager.Instance.CurrentLevelName().Contains("Het")) moveHorizontal *= -1; // upside-down
            Vector2 movement = new Vector2(moveHorizontal, 0);
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.x = Mathf.Clamp(mouseWorldPosition.x, -23, 23); // keep token inside level
            Vector3 pos = transform.localPosition;
            pos.x = mouseWorldPosition.x;
            transform.localPosition = pos;
        }
        
        if ((Input.GetKeyDown(KeyCode.Space) && !useMouseControls)
            || Input.GetMouseButtonDown(0) && useMouseControls)
        {
            rb2d.constraints = RigidbodyConstraints2D.None;
            isDropped = true;
        }
    }

    private void CheckFalling(float yPos)
    {
        if (won) return;
        
        // reset token if not moving
        if (Math.Abs(yPos - lastYPos) < yThreshold)
        {
            levelManager.Fail();
            Debug.Log("Token stuck, resetting");
        }
        lastYPos = yPos;
    }

    // checks wall collision
    private void OnCollisionEnter2D()
    {
        if (won) return;
        if (isDropped && CanPlayBounceSFX())
        {
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(bounce);
            lastBounceTime = Time.time;
        }
    }
    
    // enter win/lose zones
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (won) return;
        if (other.gameObject.CompareTag("Fail"))
        {
            levelManager.Fail();
        }
        else if (other.gameObject.CompareTag("Win"))
        {
            won = true;
            levelManager.ProgressLevel();
        }
    }

    private bool CanPlayBounceSFX()
    {
        // check if enough time has passed since the last bounce
        return Time.time - lastBounceTime >= timeBetweenBounceSFX;
    }
}
