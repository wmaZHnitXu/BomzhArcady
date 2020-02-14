using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct questStructure
{
    public byte type;
    //0 - идти
    //1 - поговорить
    //2 - принести
    public Vector3 gotuda;
    public int talkWithHim;//id перса
    public int prinesti;//id and count
    public int count;
    public string qdesc;
}
