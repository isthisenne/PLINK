using System;
using Unity.VisualScripting;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody2D rb2d;
    private bool isDropped = false;
    
    private AudioSource audioSource;
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
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            rb2d.constraints = RigidbodyConstraints2D.None;
            isDropped = true;
        }
    }

    void OnCollisionEnter2D()
    {
        audioSource.PlayOneShot(bounce);
    }
}
