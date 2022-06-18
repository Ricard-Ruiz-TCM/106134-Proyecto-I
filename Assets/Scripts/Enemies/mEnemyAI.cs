using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mEnemyAI : MonoBehaviour
{

    // ENEMY_MOV_TYPE
    // ***************
    // Enum para determinarl el tipo de movimiento del enemigo
    public enum ENEMY_MOV_TYPE
    {
        EM_SMART, EM_CARDINAL, EM_RETARD
    }

    // ENEMY_ATACK_TYPE
    // *****************
    // Enum para dterminar el tipo de ataque del enemigo
    public enum ENEMY_ATACK_TYPE
    {
        EA_MEELEE, EA_RANGE, EA_BOTH
    }

    // IA_STATE
    // ***********
    // Enum para los cerebros de los enemigos, 
    private enum IA_STATE
    {
        IA_WAITING, IA_WANDER, IA_ATTACK
    }

    // Variables que tdeterminal el estado y forma de la ia del enemigo
    private short mMovType;
    private short mAtackType;
    private short mIAState;

    // Variable para calcular el comportamiento de la ia, determinada por la distancia de detección del player
    private float mDetectionDistance;

    // Variable de estadisticas del enemigo
    private mStats mMyStats;

    // Variable para comprobar si el enemigo esta vivo
    private bool mAlive = false;

    // Gameobject del player
    private GameObject mPlayerEntity;

    // Prefab de la bullet del enemigo
    private GameObject mEnemybullet;

    // Variable para que los enemigos tengan un atack speed en concreto, y tengan que esperar para tacar de nuevo
    private bool mLetMeAtack;

    // Variable de uso temporal para calcular dirección de movimiento
    private Vector3 mDirection;

    // Variable de offset para el movimiento, margen de error
    private Vector3 mOffset = new Vector3(0.0f, -0.7f, 0.0f);

    public void init(ENEMY_MOV_TYPE mov, ENEMY_ATACK_TYPE atack, float dd)
    {
        mMovType = (short)mov;
        mAtackType = (short)atack;
        mDetectionDistance = dd;
        // Init del alguna variables propias de la IA
        mAlive = true;
        mLetMeAtack = true;
        mIAState = (short)IA_STATE.IA_WAITING;
        mMyStats = GetComponent<mEnemy>().getStats();
        mPlayerEntity = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start() 
    { 
        mEnemybullet = Resources.Load("Prefabs/Bullets/EnemyBullet") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Comprobamos si esta vivo
        if (mAlive)
        {
            // Actualizmoas IA
            updateIA();

            // Calculamos movimiento y si la distancia permite, atacamos
            enemyMovement();
        }

    }

    // updateIA
    // *********
    // Método para cambiar entre los diferents estados de la ia, según la distancia de detección
    private void updateIA()
    {

        if (canIActivate())
        {
            StartCoroutine(WaitingAfterDetect());
        }
        else if (isTimeToAtack())
        {
            StartCoroutine(WaitingBeforeAttack());
        }
        else if (mIAState != (short)IA_STATE.IA_WAITING)
        {
            if (Vector3.Distance(mPlayerEntity.transform.position, transform.position) >= mDetectionDistance * 2.5f)
            {
                mIAState = (short)IA_STATE.IA_WAITING;
            }
        }
    }

    // canIActivate
    // **************
    // @return bool true -> activate | false -> inactivo
    // Método para comprobar la distancia de detección y activar el comportamiento de la ia
    private bool canIActivate()
    {
        bool activate = false;

        if ((mAlive) && (mIAState == (short)IA_STATE.IA_WAITING))
        {
            if (Vector3.Distance(mPlayerEntity.transform.position, transform.position) <= mDetectionDistance * 2.5f)
            {
                activate = true;
            }
        }

        return activate;
    }

    // isTimeToAtack
    // **************
    // @return bool true -> ataca | false -> no ataca
    // Método para comprobar la distancia de detección y activar el comportamiento de la ia
    private bool isTimeToAtack()
    {
        bool atack = false;

        if ((mAlive) && (mIAState == (short)IA_STATE.IA_WANDER))
        {
            if (Vector3.Distance(mPlayerEntity.transform.position, transform.position) <= mDetectionDistance)
            {
                atack = true;
            }
        }

        return atack;
    }

    // enemyMovement
    // ***************
    // Parte de la ia, le da al jugador la forma de moverse según su tipo de movimiento
    private void enemyMovement()
    {

        if (mIAState == (short)IA_STATE.IA_WANDER)
        {
            basicMovement();
        }
        else if (mIAState != (short)IA_STATE.IA_WAITING)
        {
            switch (mMovType)
            {
                case (short)ENEMY_MOV_TYPE.EM_SMART:
                    smartMovement();
                    break;
                case (short)ENEMY_MOV_TYPE.EM_CARDINAL:
                    cardinalMovement();
                    break;
                case (short)ENEMY_MOV_TYPE.EM_RETARD:
                    basicMovement();
                    break;
            }
        }
    }

    // smartMovement
    // ***************
    // Movimiento del enemigo "inteligente"
    private void smartMovement()
    {
        basicMovement();
    }

    // cardinalMovement
    // ***************
    // Movimiento del enemigo solo en las direcciones cardinales
    private void cardinalMovement()
    {
        basicMovement();
    }

    // basicMovement
    // ***************
    // Movimiento del enemigo simple, sin esquivar ningún tipo de obstaculo
    private void basicMovement()
    {
        if (!canIAtack())
        {
            mDirection = (mPlayerEntity.transform.position - transform.position + mOffset).normalized;
            GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + (Vector2)(mDirection * Time.fixedDeltaTime * mMyStats.Speed));
            // Ajuste de las animaciones

            GetComponent<Animator>().SetFloat("Magnitude", mDirection.magnitude);
            GetComponent<Animator>().SetFloat("Horizontal", mDirection.x);
            GetComponent<Animator>().SetFloat("Vertical", mDirection.y);

        }
        else { enemyOffensive(); }

    }

    // canIAtack
    // **********
    // @return bool true -> puede atacar | false -> no puede atacar
    // comprueba si puede o no atacar el enemigo al player
    private bool canIAtack()
    {
        bool atack = false;

        if (Vector3.Distance(mPlayerEntity.transform.position, transform.position) <= 1.0f)
        {
            if ((mAtackType == (short)ENEMY_ATACK_TYPE.EA_MEELEE) || (mAtackType == (short)ENEMY_ATACK_TYPE.EA_BOTH))
            {
                atack = true;
            }
        }
        else if ((mAtackType == (short)ENEMY_ATACK_TYPE.EA_RANGE) || (mAtackType == (short)ENEMY_ATACK_TYPE.EA_BOTH)){ 
            atack = true; 
        } 

        return atack;
    }

    // enemyOffensive
    // ***************
    // Método para lanzar el ataque contra el player segun el tipo de ataque del enemigo
    private void enemyOffensive()
    {
        if ((mIAState == (short)IA_STATE.IA_ATTACK) && (mLetMeAtack))
        {
            switch (mAtackType)
            {
                case (short)ENEMY_ATACK_TYPE.EA_MEELEE:
                    meeleeAttack();
                    break;
                case (short)ENEMY_ATACK_TYPE.EA_RANGE:
                    rangedAttack();
                    break;
                case (short)ENEMY_ATACK_TYPE.EA_BOTH:
                    decideAtack();
                    break;
            }

            mLetMeAtack = false;
            Invoke("letMeAtack", mMyStats.AtkSpeed);
        }
    }

    // decideAtack
    // ***********
    // Método para determinar si ataca a mele o a distancia segun la distancia con el jugador
    private void decideAtack()
    {
        if (Vector3.Distance(mPlayerEntity.transform.position, transform.position) <= 1.5f)
        {
            meeleeAttack();
        } else
        {
            rangedAttack();
        }
    }

    // letMeAtack
    // ***********
    // Método de invoke para permitir al enemigo volver a atacar
    private void letMeAtack()
    {
        mLetMeAtack = true;
    }

    // rangedAttack
    // *************
    // Calculo y ejecución del ataque a distancia del enemigo
    private void rangedAttack()
    {
        GameObject bullet = Instantiate(mEnemybullet, transform.position, transform.rotation);

        // TODO: que la flecha encare al mPlayerEntity

        bullet.GetComponent<mBullet>().setDamage(mMyStats.Str);
        bullet.GetComponent<mBullet>().sendByMe(2);
    }

    // meeleeAttack
    // *************
    // Calculo y ejecución del ataque cuerpo a cuerpo del enemigo
    private void meeleeAttack()
    {
        mPlayerEntity.GetComponent<mPlayer>().hitMe(mMyStats.Str);

        GetComponent<Animator>().SetFloat("Magnitude", mDirection.magnitude);
        GetComponent<Animator>().SetFloat("Horizontal", mDirection.x);
        GetComponent<Animator>().SetFloat("Vertical", mDirection.y);
    }

    private IEnumerator WaitingAfterDetect()
    {
        yield return new WaitForSeconds(1.5f);
        mIAState = (short)IA_STATE.IA_WANDER;
    }

    private IEnumerator WaitingBeforeAttack()
    {
        yield return new WaitForSeconds(1.5f);
        mIAState = (short)IA_STATE.IA_ATTACK;
    }

}