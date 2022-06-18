using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mPauseButton : MonoBehaviour
{
    public static bool isOnPauseButton;
    public GameObject pauseText;

    void Start()
    {
        isOnPauseButton = false;
    }


    public void OnButton()
    {
        if (GameObject.FindObjectOfType<mPauseButton>())
        {
            isOnPauseButton = true;
        }
        
        pauseText.gameObject.SetActive(true);
    }

    public void OutButton()
    {
        if (GameObject.FindObjectOfType<mPauseButton>())
        {
            isOnPauseButton = false;
        }
            
        pauseText.gameObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        pauseText.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        pauseText.gameObject.SetActive(false);
    }
}
