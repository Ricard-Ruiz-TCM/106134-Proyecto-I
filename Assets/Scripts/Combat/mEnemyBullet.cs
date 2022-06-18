using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mEnemyBullet : MonoBehaviour
{
    private Vector2 target;
    private Transform playerTransform;
    private Vector3 direction;
    private Rigidbody2D theRigidbody;
    private Vector3 offset;

    // Varaible de daño básico de la flecha
    private int mDamage = 1;

    // Velocida de la flecha
    private float mSpeed = 30.0f;

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
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        theRigidbody = GetComponent<Rigidbody2D>();
        offset = new Vector3(0.0f, -0.2f, 0.0f);

        target = new Vector2(playerTransform.position.x, playerTransform.position.y);
        direction = ((playerTransform.position + offset) - transform.position).normalized;

        StartCoroutine(StartDestruction());
    }

    // Update is called once per frame
    private void Update()
    {
        // Movimiento de la flecha
        
        theRigidbody.MovePosition(transform.position + direction * mSpeed * Time.deltaTime);

        /*
        transform.position = Vector2.MoveTowards(transform.position, direction, mSpeed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            destroyMe();
        }
        */

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
        

        if ((collision.tag == "Player"))
        {
            mPlayer.GetComponent<mPlayer>().hitMe(1);
            destroyMe();
        }

        if (collision.tag == "Walls")
        {
            mScreenShake.instance.StartShake(0.2f, 0.1f);
            destroyMe();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if ((collision.gameObject.tag == "Player"))
        {
            mPlayer.GetComponent<mPlayer>().hitMe(1);
            destroyMe();
        }

        if (collision.gameObject.tag == "Walls")
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
        GameObject.Destroy(gameObject);
    }

    IEnumerator StartDestruction()
    {
        yield return new WaitForSeconds(5.0f);
        destroyMe();
    }
}

