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
    public byte type; //Можно было бы переделать в enum, но мне ленб
    /* 0 - съедобный айтем
       1 - деньги
       2 - для квестов
       3 - оружие
       4 - устанавливаемые объекты */
    public byte weapid; //Так же может использоваться для определения id префаба для постройки
    public string name;
    public string descr;
}
