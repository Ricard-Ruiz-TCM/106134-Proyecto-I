using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mDungeonTrapGenerator : MonoBehaviour
{

    // Vector de gameobjs para guardar los spanw points
    private GameObject[] mTrapSpawnPoints;

    // Vector de gameobjs para guardar los prefabs
    private GameObject[] mTrapPrefabs;

    // Int como contador de trampas
    public int mTotalTraps;

    // init
    // *****
    // Método para "activar" este script
    public void init()
    {
        // Inicializamos la trampa según la cantidad de trampas que puede haber y la cantidad de prefabs
        mTrapSpawnPoints = new GameObject[GetComponent<Transform>().Find("Traps").childCount];
        mTrapPrefabs = new GameObject[(int)mTrap.TRAP_TYPE.NO_INITIALIZED];

        mTotalTraps = 0;

        // Recuperamos los Spawn Points
        getSpawnPoints();

        // Cargamos los prefabs de trampas
        loadPrefabs();

        // Generamos las trampas
        generateTraps();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // getSpawnPoints
    // ***************
    // Método para recupear todos los spawn points de trampas
    private void getSpawnPoints()
    {
        for (int i = 0; i < mTrapSpawnPoints.Length; i++)
        {
            mTrapSpawnPoints[i] = GetComponent<Transform>().Find("Traps").GetChild(i).gameObject;
        }
    }

    // loadPrefabs
    // ************
    // Método para cargar todos los prefabs de trampas
    private void loadPrefabs()
    {
        mTrapPrefabs[0] = Resources.Load("Prefabs/Dungeon Elements/TRAP_Spike") as GameObject;
        mTrapPrefabs[1] = Resources.Load("Prefabs/Dungeon Elements/TRAP_Saw") as GameObject;
        mTrapPrefabs[2] = Resources.Load("Prefabs/Dungeon Elements/TRAP_Arrow") as GameObject;
        mTrapPrefabs[3] = Resources.Load("Prefabs/Dungeon Elements/TRAP_Clamp") as GameObject;
        mTrapPrefabs[4] = Resources.Load("Prefabs/Dungeon Elements/TRAP_Hole") as GameObject;
        mTrapPrefabs[5] = Resources.Load("Prefabs/Dungeon Elements/TRAP_Poison") as GameObject;
    }

    // generateTraps
    // **************
    // Método para decidir cuantas trampas y que trampas se van a generar
    // También las instancia en la sala
    // Tiene en cuenta el tipo de sala que es para decidir si generar o no trampas
    private void generateTraps()
    {
        if (GetComponent<mDungeonNode>().getType() != mDungeonNode.DUNGEON_NODE.DN_ENTRANCE)
        {
            int rng = Random.Range(0, (int)mTrap.TRAP_TYPE.NO_INITIALIZED);
            for (int i = 0; i < mTrapSpawnPoints.Length; i++)
            {
                // Si la room es de trampas, si o si generará la trampa en todos los spots
                if (GetComponent<mDungeonNode>().getType() == mDungeonNode.DUNGEON_NODE.DN_TRAP) gntTrp(rng, i);
                // Si no es de trampas, solo generara aleatoriamente al 50%
                else if (Random.Range(0, 2) == 0) gntTrp(rng, i);

                GameObject.Destroy(mTrapSpawnPoints[i].gameObject);
            }
        }
    }

    // gntTrp
    // *******
    // @param type Type de la trampa segun mTrap.TRAP_TYPE
    // @param i Index del mTrapSpawnPoints
    // Método interno para agilizar la generación de la trampa en el método generateTraps
    // @see mDungeonTrapGenerator.generateTraps()
    private void gntTrp(int type, int i)
    {
        GameObject tmp = Instantiate(mTrapPrefabs[type], GetComponent<Transform>().Find("Traps").GetComponent<Transform>()); ;
        tmp.GetComponent<Transform>().position = mTrapSpawnPoints[i].GetComponent<Transform>().position;
        mTrap cmp = tmp.GetComponent<mTrap>();
        if (cmp == null) cmp = tmp.GetComponent<Transform>().GetChild(0).GetComponent<mTrap>();
        cmp.setType((short)type);

    }

}
