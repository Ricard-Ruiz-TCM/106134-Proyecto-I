using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mSlowMotionBar : MonoBehaviour
{
    private Slider slowMotionBar;

    private float betweenSlowMotionTime;

    public static bool isCharging;

    public static bool isReadyToSlow;

    void Start()
    {
        isReadyToSlow = true;
        isCharging = false;

        slowMotionBar = GetComponent<Slider>();
        betweenSlowMotionTime = 60;
    }

    void Update()
    {
        if (isReadyToSlow == false)
        {
            isCharging = true;
            slowMotionBar.value = 0;
            betweenSlowMotionTime += Time.deltaTime;
            slowMotionBar.value = betweenSlowMotionTime / 60;
            StartCoroutine(WaitForNewSlowMotion());
        }
        else
        {
            betweenSlowMotionTime = 0;
            isCharging = false;
        }
    }

    IEnumerator WaitForNewSlowMotion()
    {
        yield return new WaitForSeconds(60);
        isReadyToSlow = true;
    }
}
