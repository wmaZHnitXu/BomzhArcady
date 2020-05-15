using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class quest : MonoBehaviour
{
    protected string questName;
    protected string WhatToDo {
        get {
            return whatToDo;
        }
        set {
            whatToDo = value;
            scenarioCtrl.me.NextStage(whatToDo);
        }
    }
    protected string whatToDo;
    [FormerlySerializedAs("_stage")] [SerializeField]

    private int _stage;
    public int Stage {
        get {
            return _stage;
        }
        set {
            _stage = value;
        }
    }
    
    public virtual void DialogCallback (int id) {

    }
}
