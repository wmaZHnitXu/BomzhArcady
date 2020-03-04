using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class quest : MonoBehaviour
{
    protected string questName;
    protected string whatToDo {
        get {
            return _whatToDo;
        }
        set {
            _whatToDo = value;
            scenarioCtrl.me.NextStage(_whatToDo);
        }
    }
    protected string _whatToDo;
    [SerializeField]

    private int _stage;
    public int stage {
        get {
            return _stage;
        }
        set {
            _stage = value;
        }
    }
    
    public virtual void dialogCallback (int Id) {

    }
}
