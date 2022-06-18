using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class mDungeonGenerator : MonoBehaviour {

    // Variables constantes
    // Profabs diferentes para los nodos
    public const int TOTAL_NODES = 8;
    // Tamaño de la rejilla de la dungeon 4x4
    public const int GRID = 4;

    // struct DungeonNodeBase
    // ***********************
    // Estructura para crear los nodos y comprobar información de ellos, como el tipo o si tienes salas adyacientes
    // La variable type hace referencia a lo siguiente
    // @see mDungeonNode.DUNGEON_NODE tiene en cuenta la siguiente numeración
    // 0 -> no iniciado
    // 1 -> entrada
    // 2 -> salida
    // 3 -> obstaculos
    // 4 -> trampas
    // 5 -> solución
    // 6 -> puzzle
    // 7 -> abierta
    // 8 -> tesoro
    // 9 -> camino bloqueado
    private struct DungeonNodeBase {
        public short type;
        public bool n, s, e, w;
        public GameObject obj;

        // Método para inciar las variables
        public void init() {
            type = 0; n = s = e = w = false; obj = null;
        }

        // Método para hacer set de los puntos cardinales
        public void nearby(bool N, bool S, bool E, bool W) {
            n = N; s = S; e = E; w = W;
        }
    }

    // enum DIR
    // *********
    // enumerator para "humanizar" los numeros 0, 1, 2, 3 con las direcciones cardinales
    enum DIR {
        N = 0, S = 1, E = 2, W = 3
    };

    // SpawnPoint Objects
    // *******************
    // Objectos de la escena donde se instanciarán las diferentes salas de la mazmorra
    public GameObject mSpawnPoint;
    private GameObject[,] mSpawnPoints;
    private Vector3[] mNodeSpawnPointsTransforms;

    // Dungeon Node Prefabs
    // ********************
    // Prefab de los diferentes nodos de la dungeon
    // las posiciones concretas coinciden con los prefabs definidos, en sus nombres "Room_0X" donde X es el número
    private GameObject[] mDungeonNodePrefab;

    // Dungeon Corridors Prefabs
    // **************************
    // Prefabs de los pasillos
    private GameObject mVCorridor;
    private GameObject mHCorridor;

    // mDungeonNode
    // **************
    // Dungeon con todos los parametros ya iniciados y listos para instanciarse
    private DungeonNodeBase[,] mDungeon;
    
    void Start() {

        // Recuperamos el SpawnPoint Base
        mSpawnPoint = GameObject.Find("SpawnPoints");

# if UNITY_EDITOR
        // Cambiamos al nombre del contenedor de nodos de la escena
        mSpawnPoint.name = "DungeonNodes";

#endif
        // Iniciamos la rejilla de spawn points
        mSpawnPoints = new GameObject[GRID, GRID];

        // Iniciamos el contenedor de posicion de vectores para poder re-hacer la dungeon si fuese necesario
        mNodeSpawnPointsTransforms = new Vector3[GRID * GRID];

        // Iniciamos el vector de prefabs de nodos de la dungeon
        mDungeonNodePrefab = new GameObject[TOTAL_NODES];

        // Creamos la dungoen e iniciamos sus valores
        mDungeon = new DungeonNodeBase[GRID, GRID];
        for (int x = 0; x < GRID; x++) {
            for (int y = 0; y < GRID; y++) {
                mDungeon[x, y] = new DungeonNodeBase(); mDungeon[x, y].init();
            }
        }

        // Generamos la dungeon
        generateDungeon(); 

        // Definimos que será cada nodo de la dungeon no definido ya
        defineDungeon();

#if UNITY_EDITOR
        // Mostramos el resutlado final de la dungeon
        debugNodes();
#endif
        // Localizamos e indicamos a cada nodo si tienes adyacientes y en que dirección
        setNearbyNodes();

        // Recuperamos los puntos de spawn y guardamos su posición
        getSpawnPoints();

        // Cargamos los prefabs
        loadPrefabs();

        // Instanciamos la dungoen y los pasillos
        instanceDungeonNodes();
        instanceDungeonCorridors();

        // Metemos al player en el centro de la escena de entrada
        for (int x = 0; x < GRID; x++) {
            for (int y = 0; y < GRID; y++) {
                if (mDungeon[x,y].type == (short)mDungeonNode.DUNGEON_NODE.DN_ENTRANCE) {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = mDungeon[x, y].obj.GetComponent<Transform>().position;
                }
            }
        }

    }

    // rngDir
    // ****************
    // @param n se puede añadir un valor para que el random no sea entre 0 y 4, sino entre 0 y el valor n
    // @param z se puede añadir un valor para que le random no sea entre 0 y 4, sino entre el valor z y 4
    // Método que devuelve un aleatorio entre 4 para marcar direcció cardinal por defecto
    // ****************
    // 0 -> Norte
    // 1 -> Sur
    // 2 -> Este
    // 3 -> Oeste
    // ****************
    public short rngDir(int z = 0, int n = 4) {
        return ((short)Random.Range(z, n));
    }


    // blockNext
    // **********
    // @param x posición x para analizar
    // @param y posición y para analizar
    // @return DIR dirección al que ha bloqueado
    // Método para bloquear una casilla adyaciente partiendo de una posicion x e y
    // El metodo solo podrá decidir entre 2 direcciones cardinales opuestas
    DIR blockNext(short x, short y) {

        bool n, s, e, w; n = s = e = w = false;
        DIR dir = DIR.N;

        // Comprobamos las direcciónes a las que nos podemos mover
        if ((x > 0) && (x <= 3)) w = true;
        if ((x < 3) && (x >= 0)) e = true;
        if ((y > 0) && (y <= 3)) n = true;
        if ((y < 3) && (y >= 0)) s = true;

        // comprobamos que podamos realmente movernos en esa dirección
        if ((s) && (n)) {
            if (mDungeon[x, y - 1].type != 0) {
                n = false;
            } else {
                s = false;
            }
        }
        if ((e) && (w)) {
            if (mDungeon[x + 1, y].type != 0) {
                e = false;
            } else {
                w = false;
            }
        }

        // efectuamos el movimiento a ese bloque y establecemos  la dirección en la que nos movimos
        if (n) {
            if (e) {
                if (rngDir(0, 2) == 0) { y--; dir = DIR.N; }
                else { x++; dir = DIR.E; }
            } 
            if (w) {
                if (rngDir(0, 2) == 0) { y--; dir = DIR.N; }
                else { x--; dir = DIR.W; }
            }
        } 
        if (s) {
            if (e) {
                if (rngDir(0, 2) == 0) { y++; dir = DIR.S; }
                else { x++; dir = DIR.E; }
            } 
            if (w) {
                if (rngDir(0, 2) == 0) { y++; dir = DIR.S; }
                else { x--; dir = DIR.W; }
            }
        }

        // bloqueamos la casilla
        mDungeon[x, y].type = 9;

        return dir;
    }

    // generateDungeon
    // ****************
    // Método para crear virtualmente la dungeon, bajo unas normas concretas de generación
    private void generateDungeon() {

        short x, y; x = y = 0;
        short dir = rngDir();

        // Seleccionamos una de las 4 esquinas
        switch (dir) {
            case (short)DIR.N: x = 0; y = 3; break;
            case (short)DIR.S: x = 3; y = 0; break;
            case (short)DIR.E: x = 3; y = 3; break;
            case (short)DIR.W: x = 0; y = 0; break;
            default: break;
        }

        // bloqueamos la casilla
        mDungeon[x, y].type = 9;

        // Bloqueamos la casilla adyaciente y marcamos la entrada aleatoriamente
        switch(blockNext(x, y)) {
            case DIR.N:
                if (x == 0) mDungeon[x + 1, y].type = 1; 
                else mDungeon[x - 1, y].type = 1;
                y--;
                break;
            case DIR.S:
                if (x == 0) mDungeon[x + 1, y].type = 1;
                else mDungeon[x - 1, y].type = 1;
                y++;
                break;
            case DIR.E:
                if (y == 0) mDungeon[x, y + 1].type = 1;
                else mDungeon[x, y - 1].type = 1; ;
                x++;
                break; 
            case DIR.W:
                if (y == 0) mDungeon[x, y + 1].type = 1;
                else mDungeon[x, y - 1].type = 1;
                x--;
                break;
            default: break;
        }

        // Bloqueamos casilla adyaciente y segun la dirección y posición
        // marcamos la salida del laberinto basandonos en la posición de varios bloques
        // y la dirección que tomamos, esto es posible por que la rejilla es de 4x4
        // con  una rejilla superior, las posibilidades serían mucho mayores y no sería factible
        switch (blockNext(x, y)) {
            case DIR.N: 
                y--; 
                if (y == 1) {
                    if (x == 0) mDungeon[0, 0].type = 2;
                    else mDungeon[3, 0].type = 2;
                } else {
                    if (x == 1) mDungeon[2, 3].type = 2;
                    else mDungeon[1, 3].type = 2;
                }
                break;
            case DIR.S: 
                y++;
                if (y == 2) {
                    if (x == 0) mDungeon[0, 3].type = 2;
                    else mDungeon[3, 3].type = 2;
                } else {
                    if (x == 1) mDungeon[2, 0].type = 2;
                    else mDungeon[1, 0].type = 2;
                }
                break;
            case DIR.W: 
                x--;
                if (x == 1) {
                    if (y == 0) mDungeon[0, 0].type = 2;
                    else mDungeon[0, 3].type = 2;
                } else {
                    if (y == 1) mDungeon[3, 2].type = 2;
                    else mDungeon[3, 1].type = 2;
                }
                break;
            case DIR.E: 
                x++;
                if (x == 2) {
                    if (y == 0) mDungeon[3, 0].type = 2;
                    else mDungeon[3, 3].type = 2;
                } else {
                    if (y == 1) mDungeon[0, 2].type = 2;
                    else mDungeon[0, 1].type = 2;
                }
                break;
            default: break;
        }

        // si no tiene ningun bloque central bloqeuado, rellenamos con 1 bloque aleatorio de centro
        if ((mDungeon[1,1].type == 0) && (mDungeon[1,2].type == 0) && (mDungeon[2,1].type == 0) && (mDungeon[2,2].type == 0)) {
            mDungeon[rngDir(1, 3), rngDir(1, 3)].type = 9;
        } 

        // comprobamos el centro, si tiene una bloqueada, acabamos la dungeon colocando las adyacientes bloqueadas
        if ((mDungeon[1,1].type == 9) || (mDungeon[2, 2].type == 9)) {
            if (rngDir(0, 2) == 0) mDungeon[1, 2].type = 9;
            if (rngDir(0, 2) == 0) mDungeon[2, 1].type = 9;
        } else if ((mDungeon[1,2].type == 9) || (mDungeon[2, 1].type == 9)) {
            if (rngDir(0, 2) == 0) mDungeon[1, 1].type = 9;
            if (rngDir(0, 2) == 0) mDungeon[2, 2].type = 9;
        }

        // bloqueamos la esquina que tiene 4 bloques libres comprobando las 4 esqinas
        if ((mDungeon[0, 0].type == 0) && (mDungeon[0, 1].type == 0) && (mDungeon[1, 0].type == 0) && (mDungeon[1, 1].type == 0)) {
            mDungeon[0, 0].type = 9;
        }
        if ((mDungeon[0, 3].type == 0) && (mDungeon[0, 2].type == 0) && (mDungeon[1, 3].type == 0) && (mDungeon[1, 2].type == 0)) {
            mDungeon[0, 3].type = 9;
        }
        if ((mDungeon[3, 0].type == 0) && (mDungeon[2, 0].type == 0) && (mDungeon[3, 1].type == 0) && (mDungeon[2, 1].type == 0)) {
            mDungeon[3, 0].type = 9;
        }
        if ((mDungeon[3, 3].type == 0) && (mDungeon[2, 3].type == 0) && (mDungeon[2, 2].type == 0) && (mDungeon[3, 2].type == 0)) {
            mDungeon[3, 3].type = 9;
        }

    }

    // defineDungeon
    // **************
    // Método para decidir que tipo de sala serán las salas todavía vacias
    private void defineDungeon() {

        short min = 3; 
        short[] importantNodesLeft = { 5, 6, 8 };
        short nodeX, nodeY;
        short tmp = 0;

        // Rellenamos en posiciones aleatorias las salas imprescindibles
        // Listadas a continuación
        /*
         * DN_SOLUTION = 5, 
         * DN_PUZZLE = 6, 
         * DN_TREASURE = 8, 
         */
        do {
            nodeX = rngDir(); nodeY = rngDir();
            if (mDungeon[nodeX, nodeY].type == 0) {
                mDungeon[nodeX, nodeY].type = importantNodesLeft[min - 1];
                min--;
            }
        } while (min != 0);

        // Rellenamos el resto de huecos vacios entre trampas, salas con obstaculos y salas vacias
        // con un porcenaje 50 - 20 - 30 (obstaculos, vacias, trampas)
        /*
         * DN_OBS = 3, 
         * DN_TRAP = 4,
         * DN_OPEN = 7,
         */
        for (int y = 0; y < GRID; y++) {
            for (int x = 0; x < GRID; x++) {
                if (mDungeon[x, y].type == 0) {
                    tmp = rngDir(0, 100);
                    if (tmp > 50) {
                        mDungeon[x, y].type = 3;
                    } else {
                        if (tmp < 20) {
                            mDungeon[x, y].type = 7;
                        } else {
                            mDungeon[x, y].type = 4;
                        }
                    }
                }
            }
        }

    }

#if UNITY_EDITOR
    // debugNodes
    // ***********
    // Método para debug de la creación de la tabla mDungeon
    private void debugNodes() {
        string log = "\n";
        for (int y = 0; y < GRID; y++) {
            log += "{ ";
            for (int x = 0; x < GRID; x++) {
                log += mDungeon[x, y].type.ToString() + " ";
            }
            log += "}\n";
        }
        Debug.Log(log);
    }
#endif

    // setNearbyNodes
    // ***************
    // Comprueba nodo a nodo si tienes otros nodos aydacientes para comunicarselo y tener constancia, cuantos tienes y en que direcciones
    private void setNearbyNodes() {
        bool n, s, e, w;
        for (int x = 0; x < GRID; x++) {
            for (int y = 0; y < GRID; y++) {
                n = s = e = w = false;
                if (mDungeon[x,y].type != 9) {
                    // norte
                    if (y > 0) {
                        if (mDungeon[x, y - 1].type != 9) n = true;
                    }
                    // sur
                    if (y < 3) {
                        if (mDungeon[x, y + 1].type != 9) s = true;
                    }
                    // este
                    if (x < 3) {
                        if (mDungeon[x + 1, y].type != 9) e = true;
                    }
                    // oeste
                    if (x > 0) {
                        if (mDungeon[x - 1, y].type != 9) w = true;
                    }
                    mDungeon[x, y].nearby(n, s, e, w);
                }
            }
        }
    }

    // getSpawnPoints
    // ***************
    // Método para controlar todos los hijos del objeto "NodeSpawnPoints" y "CorridorSpawnPoints" de la escena
    private void getSpawnPoints() {
        int child = 0;
        for (int x = 0; x < GRID; x++) {
            for (int y = 0; y < GRID; y++) {
                mSpawnPoints[x, y] = mSpawnPoint.GetComponent<Transform>().GetChild(child).gameObject; child++;
                mNodeSpawnPointsTransforms[x + y] = mSpawnPoints[x, y].GetComponent<Transform>().position;
            }
        }
    }

    // loadPrefabs
    // ************
    // Carga los prefabs de los nodos
    private void loadPrefabs() {
        mVCorridor = Resources.Load("Prefabs/Dungeon Nodes/V_Corridor") as GameObject;
        mHCorridor = Resources.Load("Prefabs/Dungeon Nodes/H_Corridor") as GameObject;
        for (int i = 0; i < TOTAL_NODES; i++) {
            mDungeonNodePrefab[i] = Resources.Load("Prefabs/Dungeon Nodes/Room_0" + i.ToString()) as GameObject;
        }
    }

    // instanceDungeonNodes
    // *********************
    // Método para instanciar todos los nodos de la dungeon según la tabla mDungeon en los mSpawnPoints transforms
    private void instanceDungeonNodes() {
        for (int x = 0; x < GRID; x++) {
            for (int y = 0; y < GRID; y++) {
                // Si el nodo es de dungeon isntanciamos el mapeado y adaptamos
                if (mDungeon[x, y].type != 9) { 
                    // Si tienes los nodos s, e, y w dispnibles pero no el n, instancia uno de los dos tipos de nodo especifico que engaja en esta estructura
                    // decidira aleatoriamente entre los nodos disponibles
                    if ((!mDungeon[x,y].n) && (mDungeon[x, y].s) && (mDungeon[x, y].e) && (mDungeon[x, y].w)){
                        if (rngDir(0, 2) == 0) {
                            mDungeon[x, y].obj = Instantiate(mDungeonNodePrefab[7], mSpawnPoint.GetComponent<Transform>());
                        } else {
                            mDungeon[x, y].obj = Instantiate(mDungeonNodePrefab[0], mSpawnPoint.GetComponent<Transform>());
                        }
                    } else if (!mDungeon[x, y].n) {
                        mDungeon[x, y].obj = Instantiate(mDungeonNodePrefab[0], mSpawnPoint.GetComponent<Transform>());
                    } else {
                        mDungeon[x, y].obj = Instantiate(mDungeonNodePrefab[rngDir(1,7)], mSpawnPoint.GetComponent<Transform>());
                    }

                    // Posicionamos el nodo en la escena según su punto de spawn
                    mDungeon[x, y].obj.GetComponent<Transform>().position = mSpawnPoints[x, y].GetComponent<Transform>().position;
#if UNITY_EDITOR
                    // Cambiamos el nombre al objecto en la escena
                    mDungeon[x, y].obj.name = "DungeonNode [" + x.ToString() + ", " + y.ToString() + "] - " + (mDungeonNode.DUNGEON_NODE)mDungeon[x, y].type;
#endif
                    // Indicamos a cada nodo su información para que pueda ser independiente al script de generación
                    mDungeon[x, y].obj.GetComponent<mDungeonNode>().setType(mDungeon[x, y].type);
                    mDungeon[x, y].obj.GetComponent<mDungeonNode>().setNearby(mDungeon[x, y].n, mDungeon[x, y].s, mDungeon[x, y].e, mDungeon[x, y].w);
                }
                GameObject.Destroy(mSpawnPoints[x, y].gameObject);
            }
        }
    }

    // instanceDungeonCorridors
    // *************************
    // Método para instanciar los pasillos entre los nodos de la dungeon
    // Primero comprobará los 3 primeras filas comprobando si tiene que colocar pasillos horizontales y verticales en dirección sur
    // Después comprobará la 4º fila para colocar solo pasillos horizontales
    private void instanceDungeonCorridors() {
        // Comprueba las 3 primeras filas
        for (int x = 0; x < GRID; x++) {
            for (int y = 0; y < GRID; y++){
                // Pasillo Horizontal derecha
                if (mDungeon[x, y].e) {
                    Instantiate(mHCorridor, mDungeon[x, y].obj.GetComponent<Transform>().GetChild(1).GetComponent<Transform>());
                }
                // Pasillo Vertical
                if ((mDungeon[x, y].s) && (y < GRID - 1)) {
                    Instantiate(mVCorridor, mDungeon[x, y].obj.GetComponent<Transform>().GetChild(1).GetComponent<Transform>());
                }
            }
        }
    }

}
