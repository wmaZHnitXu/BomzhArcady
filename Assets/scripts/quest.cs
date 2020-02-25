using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct stage {
    public string name;
    public string desc;
}
public class quest : MonoBehaviour
{
    public stage[] stages;
    private int _stage;
    public int stage {
        get {
            return _stage;
        }
        set {
            _stage = value;
        }
    }
    private scenarioCtrl _scenarioCtrl;
    void Start () {
        _scenarioCtrl = scenarioCtrl.me;
    }
    public virtual void dialogCallback (int Id) {

    }
}
