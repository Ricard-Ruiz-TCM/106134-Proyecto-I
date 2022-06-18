using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mTextDisplay : MonoBehaviour
{
    public GameObject infoText;

    private void Update()
    {
        if (mPauseMenu.GameIsPaused == true && infoText != null)
        {
            infoText.gameObject.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        if (mPauseMenu.GameIsPaused == false)
        {
            infoText.gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        infoText.gameObject.SetActive(false);
    }
}
