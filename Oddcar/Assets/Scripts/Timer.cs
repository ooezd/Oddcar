using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float currentTime { get; private set; }
    private bool isRunning;

    private void Awake()
    {
        currentTime = 0;
        isRunning = false;
    }

    public void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void Pause()
    {
        isRunning = false;
    }

    public void Reset()
    {
        currentTime = 0f;
        isRunning = false;
    }
}