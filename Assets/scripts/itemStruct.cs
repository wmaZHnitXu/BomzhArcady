using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct itemStruct
{
    public byte water;
    public sbyte health;
    public int exp;
    public byte eat;
    public bool flip;
    public byte type;
    public byte weapid;
    public string name;
    public string descr;
    /* 0 - съедобный айтем
       1 - деньги
       2 - для квестов
       3 - оружие */
}
