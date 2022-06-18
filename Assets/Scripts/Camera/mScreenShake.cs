﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class mScreenShake : MonoBehaviour
{
    public static mScreenShake instance;

    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;

    public float power, duration;

    public float rotationMultiplier = 15f;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (mPauseMenu.GameIsPaused == true)
        {
            StopShake();
        }
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);

        }

        transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f, 1f));
    }

    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;

        shakeRotation = power * rotationMultiplier;
    }

    public void StopShake()
    {
        shakeTimeRemaining = 0;
        shakePower = 0;

        shakeFadeTime = 0;

        shakeRotation = 0;
    }
}

