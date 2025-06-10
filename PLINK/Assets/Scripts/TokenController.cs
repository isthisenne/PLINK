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

    // the Awake() function will handle level-specific modifiers to the token
    private void Awake()
    {
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
