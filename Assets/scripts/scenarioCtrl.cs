﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenarioCtrl : MonoBehaviour
{
    public string questName;
    public bool[] scenarioData;
    public static scenarioCtrl me;
    public quest questContainer;
    [SerializeField]
    private questPanCtrl panel;
    public void NextStage () {

    }
    public void Complete () {
        
    }
}
