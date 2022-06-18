using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mPlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public Transform leftFirePoint;
    public Transform rightFirePoint;

    public GameObject[] bulletPrefabs;
    public GameObject[] arrowImages;
    private GameObject usingBullet;
    public GameObject chargingError;

    public float bulletForce = 15f;

    public static bool isReadyToShoot;

    //public static int waitForShooting = 2;

    private void Start()
    {
        isReadyToShoot = true;
        mShootingBar.isCharging = false;
        usingBullet = bulletPrefabs[0];

        arrowImages[0].SetActive(true);
        arrowImages[1].SetActive(false);
    }

    private void Update()
    {
        if (mPauseMenu.GameIsPaused == false && mPauseButton.isOnPauseButton == false)
        {
            if (mShootingBar.isCharging == false)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    usingBullet = bulletPrefabs[0];

                    arrowImages[0].SetActive(true);
                    arrowImages[1].SetActive(false);
                    arrowImages[2].SetActive(false);
                    arrowImages[3].SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    usingBullet = bulletPrefabs[1];

                    arrowImages[0].SetActive(false);
                    arrowImages[1].SetActive(true);
                    arrowImages[2].SetActive(false);
                    arrowImages[3].SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    usingBullet = bulletPrefabs[2];

                    arrowImages[0].SetActive(false);
                    arrowImages[1].SetActive(false);
                    arrowImages[2].SetActive(true);
                    arrowImages[3].SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    usingBullet = bulletPrefabs[3];

                    arrowImages[0].SetActive(false);
                    arrowImages[1].SetActive(false);
                    arrowImages[2].SetActive(false);
                    arrowImages[3].SetActive(true);
                }
            }

            if (mShootingBar.isCharging == true)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(ShowError());

                if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(ShowError());

                if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(ShowError());

                if (Input.GetKeyDown(KeyCode.Alpha4)) StartCoroutine(ShowError());

                if (Input.GetButtonDown("Fire1")) StartCoroutine(ShowError());
            }


            if (Input.GetButtonDown("Fire1") && isReadyToShoot && usingBullet == bulletPrefabs[0])
            {
                Shoot();
            }

            if (Input.GetButtonDown("Fire1") && isReadyToShoot && usingBullet == bulletPrefabs[1] && GetComponent<mPlayer>().getStats().SpecialShot0 > 0)
            {
                Shoot();
                GetComponent<mPlayer>().getStats().SpecialShot0 -= 1;
                GetComponent<mPlayer>().updateStats();
            }

            if (Input.GetButtonDown("Fire1") && isReadyToShoot && usingBullet == bulletPrefabs[2] && GetComponent<mPlayer>().getStats().SpecialShot1 > 0)
            {
                Shoot();
                GetComponent<mPlayer>().getStats().SpecialShot1 -= 1;
                GetComponent<mPlayer>().updateStats();
            }

            if (Input.GetButtonDown("Fire1") && isReadyToShoot && usingBullet == bulletPrefabs[3] && GetComponent<mPlayer>().getStats().SpecialShot2 > 0)
            {
                Shoot();
                GetComponent<mPlayer>().getStats().SpecialShot2 -= 1;
                GetComponent<mPlayer>().updateStats();
            }
        }
    }
    private void Shoot()
    {
        mAudioManager.Instance.PlaySFX("player_bow_0" + UnityEngine.Random.Range(0, 3));

        if (GetComponent<mPlayer>().getStats().Shots == 0)
        {
            GameObject bullet = Instantiate(usingBullet, firePoint.position, firePoint.rotation);
            bullet.GetComponent<mBullet>().sendByMe(1);
            isReadyToShoot = false;
            StartCoroutine(WaitForShooting());

            bullet.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str);
        }

        if (GetComponent<mPlayer>().getStats().Shots == 1)
        {
            GameObject bullet = Instantiate(usingBullet, firePoint.position, firePoint.rotation);
            GameObject bulletLeft = Instantiate(usingBullet, leftFirePoint.position, leftFirePoint.rotation);
            GameObject bulletRight = Instantiate(usingBullet, rightFirePoint.position, rightFirePoint.rotation);

            bullet.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str);
            bullet.GetComponent<mBullet>().sendByMe(1);
            bulletLeft.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str / 2);
            bulletLeft.GetComponent<mBullet>().sendByMe(1);
            bulletRight.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str / 2);
            bulletRight.GetComponent<mBullet>().sendByMe(1);

            isReadyToShoot = false;
            StartCoroutine(WaitForShooting());
        }

        if (GetComponent<mPlayer>().getStats().Shots >= 2)
        {
            GameObject bullet = Instantiate(usingBullet, firePoint.position, firePoint.rotation);
            GameObject bulletLeft = Instantiate(usingBullet, leftFirePoint.position, leftFirePoint.rotation);
            GameObject bulletRight = Instantiate(usingBullet, rightFirePoint.position, rightFirePoint.rotation);

            bullet.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str);
            bullet.GetComponent<mBullet>().sendByMe(1);
            bulletLeft.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str / 2);
            bulletLeft.GetComponent<mBullet>().sendByMe(1);
            bulletRight.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str / 2);
            bulletRight.GetComponent<mBullet>().sendByMe(1);

            StartCoroutine(ShootAgain());

            isReadyToShoot = false;
            StartCoroutine(WaitForShooting());
        }
    }

    IEnumerator WaitForShooting()
    {
        yield return new WaitForSeconds(GetComponent<mPlayer>().getStats().AtkSpeed);
        isReadyToShoot = true;
    }

    IEnumerator ShowError()
    {
        chargingError.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        chargingError.gameObject.SetActive(false);
    }

    IEnumerator ShootAgain()
    {
        yield return new WaitForSeconds(0.25f);

        GameObject bullet = Instantiate(usingBullet, firePoint.position, firePoint.rotation);
        GameObject bulletLeft = Instantiate(usingBullet, leftFirePoint.position, leftFirePoint.rotation);
        GameObject bulletRight = Instantiate(usingBullet, rightFirePoint.position, rightFirePoint.rotation);

        bullet.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str);
        bullet.GetComponent<mBullet>().sendByMe(1);
        bulletLeft.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str / 2);
        bulletLeft.GetComponent<mBullet>().sendByMe(1);
        bulletRight.GetComponent<mBullet>().setDamage(GetComponent<mPlayer>().getStats().Str / 2);
        bulletRight.GetComponent<mBullet>().sendByMe(1);
    }
}


