using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mPowerUp : MonoBehaviour
{

    // variable constante para definir el tiempo que dura un power up por defecto en segundos
    public static float mDefaulBoostTime = 2.0f;

    // PU_TYPE
    // ********
    // Enum de los tipos de power ups para ver con más claridad que PU es
    public enum PU_TYPE
    {
        PU_ARMOR = 0, PU_FOOD = 1, PU_BEER = 2, PU_RED_ARROW = 3, PU_GREEN_ARROW = 4, PU_BLUE_ARROW = 5, PU_BOOTS = 6, PU_3_ARROW = 7, PU_SPEED_ARROW = 8,
        NO_INITIALIZED
    }

    // variable para controlar el tipo de PU que es
    private short mType;

    // setType
    // ********
    // @param type tipo de PU
    // Set del typo de PU para gestionar su creación
    public void setType(short type)
    {
        mType = type;
    }

    // getType
    // ********
    // @return PU_TYPE el tipo de PU
    public PU_TYPE getType()
    {
        return (PU_TYPE)mType;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Le decimos al player que power up ha cogido
            collision.gameObject.GetComponent<mPlayer>().boostMePowerUp(mType);
            // Destruimos el objeto de power Up
            GameObject.Destroy(gameObject);
        }
    }

}
