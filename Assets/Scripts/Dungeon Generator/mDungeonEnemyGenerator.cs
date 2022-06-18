using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mDungeonEnemyGenerator : MonoBehaviour
{

    // Vector de gameobjs para guardar los spanw points
    private GameObject[] mEnemySpawnPoints;

    // Vector de gameobjs para guardar los prefabs
    private GameObject[] mEnemyPrefab;

    // init
    // *****
    // Método para "activar" este script
    public void init()
    {
        // Inicializamos la trampa según la cantidad de trampas que puede haber
        mEnemySpawnPoints = new GameObject[GetComponent<Transform>().Find("Enemies").childCount];
        mEnemyPrefab = new GameObject[(int)mEnemy.ENEMIES.TOTAL_ENEMIES];

        // Recuperamos los Spawn Points
        getSpawnPoints();

        // Cargamos los prefabs de trampas
        loadPrefabs();

        // Generamos los enemigos
        generateEnemies();
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    // getSpawnPoints
    // ***************
    // Método para recupear todos los spawn points de los enemigos
    private void getSpawnPoints()
    {
        for (int i = 0; i < mEnemySpawnPoints.Length; i++)
        {
            mEnemySpawnPoints[i] = GetComponent<Transform>().Find("Enemies").GetChild(i).gameObject;
        }
    }

    // loadPrefabs
    // ************
    // Método para cargar todos los prefabs de los enemigos
    private void loadPrefabs()
    {
        mEnemyPrefab[0] = Resources.Load("Prefabs/Enemies/Slime") as GameObject;
        mEnemyPrefab[1] = Resources.Load("Prefabs/Enemies/Spearman") as GameObject;
        mEnemyPrefab[2] = Resources.Load("Prefabs/Enemies/Archer") as GameObject;
        mEnemyPrefab[3] = Resources.Load("Prefabs/Enemies/Statue") as GameObject;
    }

    // generateEnemies
    // **************
    // Método para decidir cuantos enemigos y que enemigos se van a generar
    private void generateEnemies()
    {
        if (GetComponent<mDungeonNode>().getType() != mDungeonNode.DUNGEON_NODE.DN_ENTRANCE)
        {
            bool generate = true;
            for (int i = 0; i < mEnemySpawnPoints.Length; i++)
            {

                // Decidimos si vamos o no a genera enemigo segun la cantidad de enemigos
                if (i < 3) generate = true;
                // 25% de generar si hay menos de 5
                else if (i < 5) { if (Random.Range(0, 4) == 0) generate = true; }
                // 10% de generar si hay o más
                else { if (Random.Range(0, 10) == 0) generate = true; }

                if (generate)
                {
                    int rngEnemy = Random.Range(0, (int)mEnemy.ENEMIES.TOTAL_ENEMIES);
                    GameObject tmp = Instantiate(mEnemyPrefab[rngEnemy], GetComponent<Transform>().Find("Enemies").GetComponent<Transform>());
                    tmp.GetComponent<mEnemy>().setType((short)rngEnemy);

                    // Determinamos como serán la ia dels enemies al instanciarlos
                    switch (rngEnemy)
                    {
                        case (short)mEnemy.ENEMIES.ENEMY_SLIME:
                            tmp.GetComponent<mEnemyAI>().init(mEnemyAI.ENEMY_MOV_TYPE.EM_RETARD, mEnemyAI.ENEMY_ATACK_TYPE.EA_MEELEE, 3.0f);
                            break;
                        case (short)mEnemy.ENEMIES.ENEMY_SPEARMAN:
                            tmp.GetComponent<mEnemyAI>().init(mEnemyAI.ENEMY_MOV_TYPE.EM_SMART, mEnemyAI.ENEMY_ATACK_TYPE.EA_RANGE, 6.0f);
                            break;
                        case (short)mEnemy.ENEMIES.ENEMY_ARCHER:
                            tmp.GetComponent<mEnemyAI>().init(mEnemyAI.ENEMY_MOV_TYPE.EM_SMART, mEnemyAI.ENEMY_ATACK_TYPE.EA_BOTH, 5.0f);
                            break;
                        case (short)mEnemy.ENEMIES.ENEMY_STATUE:
                            tmp.GetComponent<mEnemyAI>().init(mEnemyAI.ENEMY_MOV_TYPE.EM_CARDINAL, mEnemyAI.ENEMY_ATACK_TYPE.EA_MEELEE, 4.0f);
                            break;
                    }

                }
                GameObject.Destroy(mEnemySpawnPoints[i].gameObject); generate = false;
            }
        }
    }

}
