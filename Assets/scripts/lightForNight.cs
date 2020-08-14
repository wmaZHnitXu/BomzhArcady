using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightForNight : MonoBehaviour
{
    [SerializeField] private MonoBehaviour lightInstance;
    void Start()
    {
        timeCtrl.me.remindAboutNight += Night;
        timeCtrl.me.remindAboutMorning += Day;
        lightInstance.enabled = timeCtrl.me.time < 300;
    }

    public void Night () {
        lightInstance.enabled = true;
    }
    public void Day () {
        lightInstance.enabled = false;
    }
}
