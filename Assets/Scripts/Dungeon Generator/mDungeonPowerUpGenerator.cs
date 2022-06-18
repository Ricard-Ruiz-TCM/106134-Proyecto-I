using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mDungeonPowerUpGenerator : MonoBehaviour
{
    // Vector de gameobjs para guardar los spanw points
    private GameObject[] mPUpawnPoints;

    // Vector de gameobjs para guardar los prefabs
    private GameObject[] mPUPrefabs;

    // init
    // *****
    // Método para "activar" este script
    public void init()
    {
        // Inicializamos la trampa según la cantidad de power ups que puede haber y la cantidad de prefabs
        mPUpawnPoints = new GameObject[GetComponent<Transform>().Find("PU").childCount];
        mPUPrefabs = new GameObject[(int)mPowerUp.PU_TYPE.NO_INITIALIZED];

        // Recuperamos los Spawn Points
        getSpawnPoints();

        // Cargamos los prefabs de Power Up
        loadPrefabs();

        // Generamos los power ups
        generatePU();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // getSpawnPoints
    // ***************
    // Método para recupear todos los spawn points de los power ups
    private void getSpawnPoints()
    {
        for (int i = 0; i < mPUpawnPoints.Length; i++)
        {
            mPUpawnPoints[i] = GetComponent<Transform>().Find("PU").GetChild(i).gameObject;
        }
    }

    // loadPrefabs
    // ************
    // Método para cargar todos los prefabs de power up
    private void loadPrefabs()
    {
        mPUPrefabs[0] = Resources.Load("Prefabs/Dungeon Elements/PU_Armor") as GameObject;
        mPUPrefabs[1] = Resources.Load("Prefabs/Dungeon Elements/PU_Food") as GameObject;
        mPUPrefabs[2] = Resources.Load("Prefabs/Dungeon Elements/PU_Beer") as GameObject;
        mPUPrefabs[3] = Resources.Load("Prefabs/Dungeon Elements/PU_Red_Arrow") as GameObject;
        mPUPrefabs[4] = Resources.Load("Prefabs/Dungeon Elements/PU_Green_Arrow") as GameObject;
        mPUPrefabs[5] = Resources.Load("Prefabs/Dungeon Elements/PU_Blue_Arrow") as GameObject;
        mPUPrefabs[6] = Resources.Load("Prefabs/Dungeon Elements/PU_Boots") as GameObject;
        mPUPrefabs[7] = Resources.Load("Prefabs/Dungeon Elements/PU_3_Arrows") as GameObject;
        mPUPrefabs[8] = Resources.Load("Prefabs/Dungeon Elements/PU_Speed_Arrow") as GameObject;
    }

    // generatePU
    // **************
    // Método para decidir cuantos power ups y que power ups se van a generar
    // También las instancia en la sala
    // Tiene en cuenta el tipo de sala que es para decidir si generar o no trampas
    private void generatePU()
    {
        for (int i = 0; i < mPUpawnPoints.Length; i++)
        {
            int rng = Random.Range(0, (int)mPowerUp.PU_TYPE.NO_INITIALIZED);
            GameObject tmp = Instantiate(mPUPrefabs[rng], GetComponent<Transform>().Find("PU").GetComponent<Transform>());

            tmp.GetComponent<Transform>().position = mPUpawnPoints[i].GetComponent<Transform>().position;
            tmp.GetComponent<mPowerUp>().setType((short)rng);

            GameObject.Destroy(mPUpawnPoints[i].gameObject);
        }
    }
}
