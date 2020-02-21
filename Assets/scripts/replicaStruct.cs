using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct replicaStruct
{
   [TextArea]
   public string repl;
   public int answerCode;
   public bool cascade; //После нее без паузы идет следующая
   public int nextReplCode;
   public int questId;
   public byte itemId;
}
