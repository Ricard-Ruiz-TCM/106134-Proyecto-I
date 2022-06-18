using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mHUD : MonoBehaviour
{
    public GameObject[] mHearts;
    public GameObject[] mArmor;

    public Text[,] mArrows;
    public Text[,] mFirstArrows;

    void Start()
    {
        mHearts = new GameObject[GameObject.Find("Canvas").GetComponent<Transform>().Find("Health").childCount];
        mArmor = new GameObject[GameObject.Find("Canvas").GetComponent<Transform>().Find("Armor").childCount];

        mArrows = new Text[GameObject.Find("Canvas").GetComponent<Transform>().Find("Arrows").childCount - 1, 3];
        mFirstArrows = new Text[GameObject.Find("Canvas").GetComponent<Transform>().Find("Arrows").childCount, 3];

        loadElements();

        updateHUD(GameObject.FindGameObjectWithTag("Player").GetComponent<mPlayer>().getStats());
    }

    void loadElements()
    {
        for (int i = 0; i < mHearts.Length; i++)
        {
            mHearts[i] = GameObject.Find("Canvas").GetComponent<Transform>().Find("Health").GetChild(i).gameObject;
        }

        for (int i = 0; i < mArmor.Length; i++)
        {
            mArmor[i] = GameObject.Find("Canvas").GetComponent<Transform>().Find("Armor").GetChild(i).gameObject;
            mArmor[i].SetActive(false);
        }

        GameObject tmp = null;
        for (int i = 0; i < mArrows.GetLength(0); i++)
        {
            tmp = GameObject.Find("Canvas").GetComponent<Transform>().Find("Arrows").GetChild(i + 1).gameObject;
            mArrows[i, 0] = tmp.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
            mArrows[i, 1] = tmp.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
            mArrows[i, 2] = tmp.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
            tmp.SetActive(false);
        }

        GameObject aaa = GameObject.Find("Canvas").GetComponent<Transform>().Find("Arrows").GetChild(0).gameObject; ;
        mFirstArrows[0, 0] = aaa.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
        mFirstArrows[0, 1] = aaa.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
        mFirstArrows[0, 2] = aaa.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
        aaa.SetActive(true);
    }

    public void updateHUD(mStats player)
    {
        for (int i = 0; i < mHearts.Length; i++)
        {
            if (player.HP > i) mHearts[i].SetActive(true);
            else mHearts[i].SetActive(false);
        }

        for (int i = 0; i < mArmor.Length; i++)
        {
            if (player.Armor > i) mArmor[i].SetActive(true);
            else mArmor[i].SetActive(false);
        }

        for (int i = 0; i < mArrows.GetLength(0); i++)
        {
            mArrows[i, 0].text = mArrows[i, 0].gameObject.transform.GetChild(0).GetComponent<Text>().text = player.SpecialShot0.ToString();
            mArrows[i, 1].text = mArrows[i, 1].gameObject.transform.GetChild(0).GetComponent<Text>().text = player.SpecialShot1.ToString();
            mArrows[i, 2].text = mArrows[i, 2].gameObject.transform.GetChild(0).GetComponent<Text>().text = player.SpecialShot2.ToString();
        }

        mFirstArrows[0, 0].text = mFirstArrows[0, 0].gameObject.transform.GetChild(0).GetComponent<Text>().text = player.SpecialShot0.ToString();
        mFirstArrows[0, 1].text = mFirstArrows[0, 1].gameObject.transform.GetChild(0).GetComponent<Text>().text = player.SpecialShot1.ToString();
        mFirstArrows[0, 2].text = mFirstArrows[0, 2].gameObject.transform.GetChild(0).GetComponent<Text>().text = player.SpecialShot2.ToString();

    }
}
