using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private Transform endPoint;
    [SerializeField, Tooltip("Time to go from start to end point")] private float period;
    
    private Vector3 start; // the start position of the platform
    private Vector3 end;
    private bool outward = true; // go away from or toward start point
    private float timer;

    private void Start()
    {
        start = transform.position;
        end =  endPoint.position;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / period);
        
        if (outward)
        {
            transform.position = Vector3.Lerp(start, end, t);

            if (timer >= period)
            {
                transform.position = end;
                timer = 0f;
                outward = false;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(end, start, t);

            if (timer >= period)
            {
                transform.position = start;
                timer = 0f;
                outward = true;
            }
        }
        endPoint.position = end; // prevent moving for gizmo
    }

    private void OnDrawGizmos()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        // Center
        Gizmos.color = Color.red;
        if (Application.isPlaying)
        {
            Gizmos.DrawSphere(start, 0.4f);
        }
        else
        {
            Gizmos.DrawSphere(transform.position, 0.4f);
        }
        // End platform
        Gizmos.color = sr.color;
        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
        Vector3 size = sr.bounds.size;
        Gizmos.DrawCube(endPoint.position, size);
        // Center (end)
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPoint.position, 0.4f);
        // Path
        Gizmos.color = Color.yellow;
        if (Application.isPlaying)
        {
            Gizmos.DrawLine(start, endPoint.position);
        }
        else
        {
            Gizmos.DrawLine(transform.position, endPoint.position);
        }
    }
}
