using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mPlayer : MonoBehaviour
{

    public Image hitEffect;

    // Variable para controlar si peude ser golpeado de nuevo o no
    private bool mHitMeAgain = true;

    // Variable para controlar las estadisticas del player
    mStats mPlayerStats;

    // Variable para controlar si el player esta cayendo con más facilidad
    private bool mFalling;

    // variable de HUD del juego
    private mHUD mHud;

    // Start is called before the first frame update
    void Start()
    {
        // Creamos las Stats del player y rellenamos con valores definios
        mPlayerStats = new mStats();
        mPlayerStats.load(6, 0, 100, 10, 5.0f, 2.0f, 0, 0, 0, 0);

        // Recuperamos el HUD
        mHud = GameObject.Find("Canvas").GetComponent<mHUD>(); //UNCOMEN

        // No cae
        mFalling = false;

        // Hacemos el body del player dynamico
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // getStats
    // *********
    // @return mStats mPlayerStats
    // Método para recuperar las stats del player
    public mStats getStats()
    {
        return mPlayerStats;
    }

    // boostMePowerUp
    // ***************
    // @param putype Type del power up
    // Método para actualizar las stadisticas según el power up que se haya cogido
    public void boostMePowerUp(short putype)
    {
        switch (putype)
        {
            case (short)mPowerUp.PU_TYPE.PU_ARMOR:
                mAudioManager.Instance.PlaySFX("pu_armor");
                mPlayerStats.Armor += 1;
                if (mPlayerStats.Armor > 4) mPlayerStats.Armor = 4;
                break;
            case (short)mPowerUp.PU_TYPE.PU_FOOD:
                mAudioManager.Instance.PlaySFX("pu_food");
                mPlayerStats.HP += 1;
                if (mPlayerStats.HP > 6) mPlayerStats.HP = 6;
                break;
            case (short)mPowerUp.PU_TYPE.PU_BEER:
                mAudioManager.Instance.PlaySFX("pu_beer", 0.35f);
                mPlayerStats.Str += 1;
                break;
            case (short)mPowerUp.PU_TYPE.PU_RED_ARROW:
                mAudioManager.Instance.PlaySFX("pu_arrow");
                mPlayerStats.SpecialShot0 += 1;
                break;
            case (short)mPowerUp.PU_TYPE.PU_GREEN_ARROW:
                mAudioManager.Instance.PlaySFX("pu_arrow");
                mPlayerStats.SpecialShot1 += 1;
                break;
            case (short)mPowerUp.PU_TYPE.PU_BLUE_ARROW:
                mAudioManager.Instance.PlaySFX("pu_arrow");
                mPlayerStats.SpecialShot2 += 1;
                break;
            case (short)mPowerUp.PU_TYPE.PU_BOOTS:
                mAudioManager.Instance.PlaySFX("pu_boots");
                mPlayerStats.Speed += 2.0f; Invoke("slowMe", mPowerUp.mDefaulBoostTime);
                break;
            case (short)mPowerUp.PU_TYPE.PU_3_ARROW:
                mAudioManager.Instance.PlaySFX("pu_arrow");
                mPlayerStats.Shots += 1;
                if (mPlayerStats.Shots > 3) mPlayerStats.Armor = 3;
                break;
            case (short)mPowerUp.PU_TYPE.PU_SPEED_ARROW:
                mAudioManager.Instance.PlaySFX("pu_arrow");
                mPlayerStats.AtkSpeed -= 0.25f;
                if (mPlayerStats.AtkSpeed <= 0.25f) mPlayerStats.AtkSpeed = 0.25f;
                break;
            default: break;
        }

        updateStats();
    }

    // slowMe
    // *******
    // Método para reducir la velocidad del player una vez se acaba el boost de las botas
    private void slowMe()
    {
        mPlayerStats.Speed -= 2.0f;
    }

    // hitMeTrap
    // **********
    // @param traptype Type de la trampa
    // Método que gestiona el daño a recibir según la trampa
    public void hitMeTrap(short traptype)
    {
        switch (traptype)
        {
            case (short)mTrap.TRAP_TYPE.TRAP_SPIKE:
                mAudioManager.Instance.PlaySFX("trap_spike", 0.5f);
                hitMe(1);
                break;
            case (short)mTrap.TRAP_TYPE.TRAP_SAW:
                mAudioManager.Instance.PlaySFX("trap_metal", 0.5f);
                hitMe(2);
                break;
            case (short)mTrap.TRAP_TYPE.TRAP_CLAMP:
                mAudioManager.Instance.PlaySFX("trap_clamp", 0.5f);
                hitMe(2);
                break;
            case (short)mTrap.TRAP_TYPE.TRAP_HOLE:
                mAudioManager.Instance.PlaySFX("trap_fall", 0.5f);
                if (!mFalling) fall();
                break;
            case (short)mTrap.TRAP_TYPE.TRAP_POISON:
                mAudioManager.Instance.PlaySFX("trap_poison", 0.5f);
                hitMe(1);
                break;
            default: break;
        }

    }

    // hitMe
    // ******
    // @param damage daño a recibir
    // Método para recibir daño siendo el player desde cualquier input
    public void hitMe(int damage)
    {
        if (mHitMeAgain)
        {
            mHitMeAgain = false;

            // Efectos de pantalla y camar
            mScreenShake.instance.StartShake(0.2f, 0.5f);
            hitEffect.gameObject.SetActive(true);

            // Le quitamos vida al jugador
            if (mPlayerStats.Armor > 0) mPlayerStats.Armor -= damage;
            if (mPlayerStats.Armor < 0)
            {
                mPlayerStats.HP += mPlayerStats.Armor;
                mPlayerStats.Armor = 0;
            }
            else mPlayerStats.HP -= damage;
            
            // Le hacemos invencible por un segudno y medio
            Invoke("invincible", 0.5f);
            hitEffect.gameObject.SetActive(true);

            // Actualizamos el HUD con la nueva información
            updateStats();
        }
    }

    // fall
    // *****
    // Método para controlar cuando el player cae al vacio por una trampa
    public void fall()
    {
        mPlayerStats.Armor = 0; mFalling = true;
        GetComponent<Animator>().SetBool("IsDead", true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Invoke("afterFall", 1.15f);
    }

    // updateStats
    // ************
    // Método para actualizar el hud con la informaicón del player
    public void updateStats()
    {
        mHud.updateHUD(mPlayerStats);
    }

    // afterFall
    // **********
    // Método para controlar la recuperación dle player una vez ya ha acaado la animación de caer
    private void afterFall()
    {
        GetComponent<Animator>().SetBool("IsDead", false);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        hitMe(2); Invoke("fallAgain", 1.0f);
    }

    // fallAgain
    // **********
    // Método para darle al jugador 1s de tiempo una vez cae
    private void fallAgain()
    {
        mFalling = false;
    }

    // invincible
    // **********
    // Método en el que el player desactiva el efecto de golpe e invoka el método para poder ser golpeado de nuevo
    private void invincible()
    {
        hitEffect.gameObject.SetActive(false);
        Invoke("hitMeAgain", 1.0f);
    }

    // hitMeAgain
    // ***********
    // Setter para la variable referente a la capacidad del jugador de recibir un golpe
    private void hitMeAgain()
    {
        mHitMeAgain = true;
    }

    // gameOver
    // *********
    // @return bool true -> ha muerto | false -> sigue vivo
    // Método para comprobar si el player ha perdido todas sus vidas
    public bool gameOver()
    {
        if (mPlayerStats.HP < 0) return true;
        return false;
    }


}
