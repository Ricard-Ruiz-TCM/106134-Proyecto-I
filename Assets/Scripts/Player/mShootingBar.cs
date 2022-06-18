using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mShootingBar : MonoBehaviour
{
    private Slider shootingBar;

    private float betweenShootingTime;

    public static bool isCharging;

    void Start()
    {
        isCharging = false;
        shootingBar = GetComponent<Slider>();
        //betweenShootingTime = 2.0f;
        betweenShootingTime = 0.0f;
    }

    void Update()
    {
        if (mPlayerShooting.isReadyToShoot == false)
        {
            isCharging = true;
            shootingBar.value = 0;
            betweenShootingTime += Time.deltaTime;
            shootingBar.value = betweenShootingTime / GameObject.FindGameObjectWithTag("Player").GetComponent<mPlayer>().getStats().AtkSpeed;
        }
        else
        {
            betweenShootingTime = 0.0f;
            isCharging = false;
        }
    }
}
