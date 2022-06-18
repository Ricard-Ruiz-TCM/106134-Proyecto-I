using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mEnemy : MonoBehaviour
{
    public GameObject killEffect;

    // enum ENEMIES
    // ****************
    // Define el comportamiento de ataque del enemigo
    public enum ENEMIES
    {
        ENEMY_SLIME, ENEMY_SPEARMAN, ENEMY_ARCHER, ENEMY_STATUE,
        TOTAL_ENEMIES
    }

    // Stats del enemy
    private mStats mEnemyStats;

    // variables que definen el tipo de enemy
    private short mType;

    // Start is called before the first frame update
    void Start()
    {
        mType = 3;  whoIAm();

        //killEffect = Resources.Load("Prefabs/Bullets/Blood") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // getStats
    // *********
    // @return mStats mEnemyStats
    // Método para recuperar las stats del enemy
    public mStats getStats()
    {
        return mEnemyStats;
    }

    // hitMe
    // ******
    // @param damage daño a recibir
    // Método para recibir daño siendo el enemy desde el player
    public void hitMe(int damage)
    {
        mScreenShake.instance.StartShake(0.2f, 0.1f);

        mEnemyStats.HP -= 1;
        if (mEnemyStats.HP <= 0)
        {
            StartCoroutine(BloodEffect());
            destroyMe();
        }
    }

    // setType
    // ********
    // @param type tipo de enemigo
    // Set del typo de enemigo para gestionar su creación, rellena sus stats
    public void setType(short type)
    {
        mType = type; whoIAm();
    }

    // whoIAm
    // *******
    // Método para darle las stats apropiadas al enemigo
    private void whoIAm()
    {
        // Iniciamos las stats
        mEnemyStats = new mStats();

        // Cargamos stats segun el tipo de enemigo
        switch (mType)
        {
            case (short)ENEMIES.ENEMY_SLIME:
                mEnemyStats.load(3, 0, 1, 7, 2.0f, 2.0f, 1, 0, 0, 0);
                break;
            case (short)ENEMIES.ENEMY_SPEARMAN:
                mEnemyStats.load(4, 0, 1, 10, 2.5f, 2.0f, 1, 0, 0, 0);
                break;
            case (short)ENEMIES.ENEMY_ARCHER:
                mEnemyStats.load(2, 0, 1, 20, 3.0f, 1.0f, 1, 0, 0, 0);
                break;
            case (short)ENEMIES.ENEMY_STATUE:
                mEnemyStats.load(5, 0, 1, 5, 2.0f, 2.0f, 1, 0, 0, 0);
                break;
            default: break;
        }
    }

    // getType
    // ********
    // @return ENEMIES el tipo de enemigo
    public ENEMIES getType()
    {
        return (ENEMIES)mType;
    }

    // destroyMe();
    // ************
    // Método interno para destruir el objeto
    private void destroyMe()
    {
        GameObject.Destroy(gameObject);
    }

    IEnumerator BloodEffect()
    {
        GameObject effect = Instantiate(killEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Destroy(effect);
    }
}
