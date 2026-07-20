using System;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    [Header("Values")]
    public float moveSpeed = 10f;
    public float leftEdge = -25f;
    public float rightEdge = 25f;
    
    private LevelManager levelManager;
    private Rigidbody2D rb2d;
    private bool isDropped = false;
    private bool useMouseControls = true; // whether input is using mouse or keyboard controls
    public static event Action<TokenController> OnTokenDropped;
    
    // used for secondary loss condition
    [Header("Token Stuck Timeout")]
    public float yThreshold = 3f;
    public float heightCheckInterval = 2f;
    private float lastYPos = -100f;
    private float heightCheckTimer = 0f;
    
    [Header("Effects")]
    public AudioClip bounce;
    public float timeBetweenBounceSFX = 0.1f;
    private AudioSource audioSource;
    private float lastBounceTime = -Mathf.Infinity;
    
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

            if(GameManager.Instance.CurrentLevelName().Contains("Nun"))
                HandleWarp();

            if (Input.GetKeyDown(KeyCode.R))
            {
                LevelFail();
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
            OnTokenDropped?.Invoke(this);
        }
    }

    private void CheckFalling(float yPos)
    {
        if (won) return;
        
        // reset token if not moving
        if (Math.Abs(yPos - lastYPos) < yThreshold)
        {
            LevelFail();
            Debug.Log("Token stuck, resetting");
        }
        lastYPos = yPos;
    }

    // warp token to other side of screen on Nun
    private void HandleWarp()
    {
        Vector3 pos = transform.position;
        float width = GetComponent<SpriteRenderer>().bounds.extents.x;
        
        if(pos.x - width < leftEdge) pos.x = rightEdge - width;
        if(pos.x + width > rightEdge) pos.x = leftEdge + width;
        
        transform.position = pos;
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
            LevelFail();
        }
        else if (other.gameObject.CompareTag("Win"))
        {
            LevelWin();
        }
    }

    public void LevelFail()
    {
        levelManager.Fail();
    }

    private void LevelWin()
    {
        won = true;
        levelManager.ProgressLevel();
    }

    private bool CanPlayBounceSFX()
    {
        // check if enough time has passed since the last bounce
        return Time.time - lastBounceTime >= timeBetweenBounceSFX;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        // Left edge
        Gizmos.DrawLine(
            new Vector3(leftEdge, -20, 0),
            new Vector3(leftEdge, 20, 0)
        );

        // Right edge
        Gizmos.DrawLine(
            new Vector3(rightEdge, -20, 0),
            new Vector3(rightEdge, 20, 0)
        );
    }
}
