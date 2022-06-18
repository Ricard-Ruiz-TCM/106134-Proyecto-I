using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mGameManage : MonoBehaviour
{
    public GameObject slowMotionImage;

    float fixedDaltaTime;

    public GameObject chargingError;

    public static bool isSlowMotion;

    Scene scene;

    private void Start()
    {
        fixedDaltaTime = Time.fixedDeltaTime;
        Time.timeScale = 1;

        //scene = SceneManager.GetSceneAt(1);

        isSlowMotion = false;
    }

    void Update()
    {
        if (mPauseMenu.GameIsPaused == false)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<mPlayer>().gameOver())
                {
                    StartCoroutine(Death());
                }
            }

            if (Input.GetKeyDown(KeyCode.F) && mSlowMotionBar.isCharging != true)
            {
                StartCoroutine(SlowMotion());
                mSlowMotionBar.isReadyToSlow = false;
            }

            if (mSlowMotionBar.isCharging == true)
            {
                if (Input.GetKeyDown(KeyCode.F)) StartCoroutine(ShowSlowMotionError());
            }

            if (scene.name == "Level")
            {
                if (GameObject.FindGameObjectWithTag("Enemy") == null)
                {
                    SceneManager.LoadScene("Win");
                }
            }
        } 
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Death");
    }

    IEnumerator SlowMotion()
    {
        isSlowMotion = true;
        slowMotionImage.gameObject.SetActive(true);
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = fixedDaltaTime * Time.timeScale;
        yield return new WaitForSecondsRealtime(4);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = fixedDaltaTime * Time.timeScale;
        slowMotionImage.gameObject.SetActive(false);
        isSlowMotion = false;
    }

    IEnumerator ShowSlowMotionError()
    {
        chargingError.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        chargingError.gameObject.SetActive(false);
    }
}
