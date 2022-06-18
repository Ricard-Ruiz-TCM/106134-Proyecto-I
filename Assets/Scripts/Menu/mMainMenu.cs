using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class mMainMenu : MonoBehaviour
{
    int buttonNumber;
    public GameObject optionsMenu, mainMenu, graphicsMenu, soundMenu;

    public void Play()
    {
        buttonNumber = 1;
        StartCoroutine(ButtonWaitTime());
    }

    public void Quit()
    {
        buttonNumber = 2;
        StartCoroutine(ButtonWaitTime());
    }

    public void Options()
    {
        buttonNumber = 3;
        StartCoroutine(ButtonWaitTime());
    }

    public void BackToMenu()
    {
        buttonNumber = 4;
        StartCoroutine(ButtonWaitTime());
    }

    public void BackToOptions()
    {
        buttonNumber = 5;
        StartCoroutine(ButtonWaitTime());
    }

    public void Graphics()
    {
        buttonNumber = 6;
        StartCoroutine(ButtonWaitTime());
    }

    public void Sound()
    {
        buttonNumber = 7;
        StartCoroutine(ButtonWaitTime());
    }

    IEnumerator ButtonWaitTime()
    {
        yield return new WaitForSeconds(0.5f);

        if(buttonNumber == 1) SceneManager.LoadScene("Level");

        if (buttonNumber == 2) {Application.Quit(); Debug.Log("Quit");}

        if (buttonNumber == 3) { optionsMenu.gameObject.SetActive(true); mainMenu.gameObject.SetActive(false); }

        if (buttonNumber == 4) { optionsMenu.gameObject.SetActive(false); mainMenu.gameObject.SetActive(true); }

        if (buttonNumber == 5) { optionsMenu.gameObject.SetActive(true); graphicsMenu.gameObject.SetActive(false); soundMenu.gameObject.SetActive(false); }

        if (buttonNumber == 6) { graphicsMenu.gameObject.SetActive(true); optionsMenu.gameObject.SetActive(false); }

        if (buttonNumber == 7) { soundMenu.gameObject.SetActive(true); optionsMenu.gameObject.SetActive(false); }

        buttonNumber = 0;
    }
}
