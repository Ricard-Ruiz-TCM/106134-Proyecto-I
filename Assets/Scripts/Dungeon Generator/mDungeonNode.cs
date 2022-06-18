using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mDungeonNode : MonoBehaviour {

    // enum DUNGEON_NODE
    // ******************
    // enumrator para "humanizar" los tipos de nodos de la mazmorra
    public enum DUNGEON_NODE {
        DN_CLEAR = 0, DN_ENTRANCE = 1, DN_EXIT = 2, DN_OBS = 3, DN_TRAP = 4, DN_SOLUTION = 5, DN_PUZZLE = 6, DN_OPEN = 7, DN_TREASURE = 8, DN_BLOCK = 9
    }

    // Type
    // *****
    // Tipo de nodo, para la gestión de objetos y otras cosas
    private short mType;

    // Cardinales
    // ***********
    // Bools para determinar si tienes nodos adyacientes y en que direcciones
    private bool mNorth, mSouth, mEst, mWest;

    // Init
    void Start() {
        mNorth = mSouth = mEst = mWest = false;
        mType = (short)DUNGEON_NODE.DN_CLEAR;
    }

    // setNearby
    // ***********
    // @param n Nodo norte
    // @param s Nodo sud
    // @param e Nodo este
    // @param w Nodo oeste
    // Define si este nodo tiene nodos adyacientes
    public void setNearby(bool n, bool s, bool e, bool w) {
        mNorth = n; mSouth = s; mEst = e; mWest = w;
    }

    // getNort
    // ********
    // @return bool si tiene nodo norte
    public bool getNort() {
        return mNorth;
    }

    // getSouth
    // ********
    // @return bool si tiene nodo sur
    public bool getSouth (){
        return mSouth;
    }

    // getEst
    // ********
    // @return bool si tiene nodo este
    public bool getEst() {
        return mEst;
    }

    // getWest
    // ********
    // @return bool si tiene nodo oeste
    public bool getWest() {
        return mWest;
    }

    // setType
    // ********
    // @param type tipo de nodo
    // Set del typo del nodo para gestionar su creación
    public void setType(short type) {
        mType = type;
        // Genera los power ups, los enemigos y las trampas
        generateAll();
    }

    // generateAll
    // ************
    // Método para generar trampas, enemigos y power ups una vez los nodos tienen tipo
    private void generateAll()
    {
        GetComponent<mDungeonTrapGenerator>().init();
        GetComponent<mDungeonEnemyGenerator>().init();
        GetComponent<mDungeonPowerUpGenerator>().init();
    }

    // getType
    // ********
    // @return DUNGEON_NODE el tipo del nodo
    public DUNGEON_NODE getType() {
        return (DUNGEON_NODE)mType;
    }
}
