using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcManager : MonoBehaviour
{
    public GameObject[] npcPrefabs;
    public static npcManager me;
    void Start () {
        me = this;
    }
    public GameObject GetNpcWithId(int id) {
        return npcPrefabs[id];
    }
}
