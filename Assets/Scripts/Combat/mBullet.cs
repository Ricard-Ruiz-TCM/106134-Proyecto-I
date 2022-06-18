using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mBullet : MonoBehaviour
{
    // Varaible de daño básico de la flecha
    private int mDamage = 1;

    // Velocida de la flecha
    private float mSpeed = 10.5f;

    // GameObject Player para el disparo
    private GameObject mPlayer;

    // Variable para determinar quien manda la flecha
    // 0 -> nadie
    // 1 -> player
    // 2 -> enemy
    private int mSendBy;

    // Start is called before the first frame update
    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        // Movimiento de la flecha
        transform.position += transform.up * mSpeed * Time.deltaTime;

        // Destruye la bala si se aleja demasidado del player
        if (Vector3.Distance(mPlayer.GetComponent<Transform>().position, transform.position) > 30) destroyMe();
    }

    // sendByMe
    // *********
    // @param int n creador
    // setea quien manda la flehca
    public void sendByMe(int n)
    {
        mSendBy = n;
    }

    // OnTriggerEnter2D is called on trigger collision detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Enemy") && (mSendBy != 2))
        {
            collision.GetComponent<mEnemy>().hitMe(1);
            destroyMe();
        }

        if ((collision.tag == "Player") && (mSendBy != 1) && gameObject.name == "TrapArrow")
        {
            mPlayer.GetComponent<mPlayer>().hitMe(mDamage);
            destroyMe();
        }

        if (collision.tag == "Walls" && gameObject.name != "TrapArrow")
        {
            mScreenShake.instance.StartShake(0.2f, 0.1f);
            destroyMe();
        }
    }

    // getDamage
    // **********
    // @return int mDamage daño de la flecha
    // Método getter del damage
    public int getDamage()
    {
        return mDamage;
    }

    // setDamage
    // **********
    // @param int damage Nuevo daño de la flehca
    // Método setter del damage
    public void setDamage(int damage)
    {
        mDamage = damage;
    }

    // destroyMe();
    // ************
    // Método interno para destruir el objeto
    private void destroyMe()
    {
        mAudioManager.Instance.PlaySFX("arrow_hit", 0.75f);
        GameObject.Destroy(gameObject);
    }
}
