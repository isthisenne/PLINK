using System;
using System.Collections;
using UnityEngine;

public class ZayinExplode : MonoBehaviour
{
    private void OnEnable()  => TokenController.OnTokenDropped += HandleTokenDropped;
    private void OnDisable() => TokenController.OnTokenDropped -= HandleTokenDropped;
    
    [SerializeField] private float timeUntilExplode = 15f;
    
    private float timer;
    private int lastWholeSecond = -1;
    
    private TokenController currentToken;
    private bool dropped = false;

    private void HandleTokenDropped(TokenController token)
    {
        dropped = true;
        currentToken = token;
        timer = timeUntilExplode;
    }

    private void Update()
    {
        if (!dropped) return;
        timer -= Time.deltaTime;

        int currentSecond = Mathf.FloorToInt(timer);
        if (currentSecond != lastWholeSecond)
        {
            lastWholeSecond = currentSecond;
            Debug.Log($"Zayin Explosion Timer: {currentSecond}");
        }

        if (timer <= 0 || currentToken == null)
        {
            Debug.Log("TOKEN FUCKING EXPLODED!");
            currentToken.LevelFail();
            currentToken = null;
            dropped = false;
        }
    }
}
