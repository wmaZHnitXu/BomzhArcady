using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenarioCtrl : MonoBehaviour
{
    private int _stage;
    public int stage {
        get {
            return _stage;
        }
        set {
            _stage = value;
        }
    }
    public bool[] scenarioData;
    public static scenarioCtrl me;
    void Start()
    {
        me = this;
    }

    void Update()
    {
        
    }
}
