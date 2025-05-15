using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody2D rb2d;
    private bool isDropped = false;
    
    private AudioSource audioSource;
    public float timeBetweenBounceSFX = 0.1f;
    private float lastBounceTime = -Mathf.Infinity;
    public AudioClip bounce;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
        
        if (Input.GetKey(KeyCode.Space))
        {
            rb2d.constraints = RigidbodyConstraints2D.None;
            isDropped = true;
        }
    }

    private void OnCollisionEnter2D()
    {
        if (isDropped && CanPlayBounceSFX())
        {
            audioSource.PlayOneShot(bounce);
            lastBounceTime = Time.time;
        }
    }

    private bool CanPlayBounceSFX()
    {
        // check if enough time has passed since the last bounce
        return Time.time - lastBounceTime >= timeBetweenBounceSFX;
    }
}
