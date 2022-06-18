using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mTrap : MonoBehaviour
{

    // TRAP_TYPE
    // ********
    // Enum de los tipos de trampas para ver con más claridad que trap es
    public enum TRAP_TYPE
    {
        TRAP_SPIKE = 0, TRAP_SAW = 1, TRAP_ARROW = 2, TRAP_CLAMP = 3, TRAP_HOLE = 4, TRAP_POISON = 5,
        NO_INITIALIZED
    }

    // variable para controlar el tipo de trampa que es
    private short mType;

    // Variables para controlar la velocidad y la velocidad rotaicón de la sierra
    private float mSawSpeed;

    // variable parar comprobar si la placa de presion esta activa y puede activarse
    private bool mArrowActive;

    // prefab de las flehcas que lanza la trampa de flechas
    private GameObject mArrowPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // set de la velocidad
        mSawSpeed = 2.5f;

        // set de la placa
        mArrowActive = false;

        // cargamos el prefab de la flecha
        mArrowPrefab = Resources.Load("Prefabs/Bullets/TrapArrow") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (mType == (short)TRAP_TYPE.TRAP_SAW)
        {
            GetComponent<Transform>().localPosition = new Vector3(Mathf.Sin(Time.time * 2) * mSawSpeed, 0.5f, 0.0f);
            GetComponent<Transform>().Rotate(new Vector3(0, 0, (Mathf.Sin(Time.time * 2) * 10.0f)));
        }
    }

    // setType
    // ********
    // @param type tipo de trap
    // Set del typo de trampa para gestionar su creación
    public void setType(short type)
    {
        mType = type;
    }

    // getType
    // ********
    // @return PU_TYPE el tipo de PU
    public TRAP_TYPE getType()
    {
        return (TRAP_TYPE)mType;
    }

    // OnCollisionEnter2D is called on collision detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<mPlayer>().hitMeTrap(mType);

            if (mType == (short)TRAP_TYPE.TRAP_CLAMP)
            {
                GetComponent<Animator>().SetBool("active", true);
                Invoke("destroyMe", 0.25f);
            }
        }
    }

    // OnTriggerEnter2D is called on trigger collision detection
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if ((mType == (short)TRAP_TYPE.TRAP_ARROW) && (!mArrowActive))
            {
                mArrowActive = true;
                shootArrows(collider.gameObject);
            }
            collider.gameObject.GetComponent<mPlayer>().hitMeTrap(mType);
        }
    }

    // OnTriggerExit2D is called on trigger stay collision detection
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (mType == (short)TRAP_TYPE.TRAP_POISON)
            {
                collider.gameObject.GetComponent<mPlayer>().hitMeTrap(mType);
            }
        }
    }

    // OnTriggerExit2D is called on trigger exit collision detection
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if ((mType == (short)TRAP_TYPE.TRAP_ARROW) && (mArrowActive))
            {
                shootArrows(collider.gameObject); mArrowActive = false;
            }
        }
    }

    // shootArrows
    // ************
    // @param player GameObject del player
    // Método para disparar una tromba de flechas en dirección al player desde un angulo aleatorio
    private void shootArrows(GameObject player)
    {

        float direction = Random.Range(0.0f, 360.0f);

        GameObject bullet = Instantiate(mArrowPrefab, player.transform);

        bullet.transform.localEulerAngles = new Vector3(0.0f, 0.0f, direction);

        bullet.transform.position -= bullet.transform.up * 5.0f;

        bullet.GetComponent<CircleCollider2D>().radius = 0.08f;

        Invoke("canTrapYouAgain", 5.0f);

        StartCoroutine(DestroyBullet(bullet));

    }

    // canTrapYouAgain
    // ****************
    // Método para permitirle a la trampa de felchas volver a disparar
    private void canTrapYouAgain()
    {
        mArrowActive = true;
    }

    // destroyMe();
    // ************
    // Método interno para destruir el objeto
    private void destroyMe()
    {
        GameObject.Destroy(gameObject);
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(4.0f);
        Destroy(bullet);
    }

}
