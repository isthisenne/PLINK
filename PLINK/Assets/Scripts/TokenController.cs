using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class TokenController : MonoBehaviour
{
    private LevelManager levelManager;
    public float moveSpeed = 10f;
    private Rigidbody2D rb2d;
    private bool isDropped = false;
    
    private AudioSource audioSource;
    public float timeBetweenBounceSFX = 0.1f;
    private float lastBounceTime = -Mathf.Infinity;
    public AudioClip bounce;
    
    // used for secondary loss condition
    private float lastYPos = -100f;
    private float yThreshold = 3f;
    private float heightCheckInterval = 2f;
    private float heightCheckTimer = 0f;

    // the Awake() function will handle level-specific modifiers to the token
    private void Awake()
    {
        // bet size modifier
        if (GameManager.Instance.CurrentLevel().Equals("Bet_1") ||
            GameManager.Instance.CurrentLevel().Equals("Bet_2") ||
            GameManager.Instance.CurrentLevel().Equals("Bet_3"))
        {
            this.transform.localScale = new Vector3(0.5f, 0.5f);
        }
        else
        {
            this.transform.localScale = Vector3.one;
        }
        
        // gimel bounce modifier
        PhysicsMaterial2D physicsMaterial = new PhysicsMaterial2D();
        physicsMaterial.friction = 0.4f;
        physicsMaterial.bounciness = 0.3f;
        
        if (GameManager.Instance.CurrentLevel().Equals("Gimel_1") ||
            GameManager.Instance.CurrentLevel().Equals("Gimel_2") ||
            GameManager.Instance.CurrentLevel().Equals("Gimel_3"))
        {
            physicsMaterial.bounciness = 0.8f;
        }
        GetComponent<CircleCollider2D>().sharedMaterial = physicsMaterial;
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
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
    }

    private void HandleMovement()
    {
        // move token based on horizontal input
        float moveHorizontal = Input.GetAxis ("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb2d.constraints = RigidbodyConstraints2D.None;
            isDropped = true;
        }
    }

    private void CheckFalling(float yPos)
    {
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
        if (other.gameObject.CompareTag("Fail"))
        {
            levelManager.Fail();
        }
        else if (other.gameObject.CompareTag("Win"))
        {
            levelManager.ProgressLevel();
        }
    }

    private bool CanPlayBounceSFX()
    {
        // check if enough time has passed since the last bounce
        return Time.time - lastBounceTime >= timeBetweenBounceSFX;
    }
}
